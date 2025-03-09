using AirbnbREST.Models;
namespace AirbnbREST.Repositories;

public class InMemoryBookingRepository : InMemoryRepository<Booking>, IBookingRepository
{
    private readonly IAvailabilityRepository _availabilityRepository;

    public InMemoryBookingRepository(IAvailabilityRepository availabilityRepository)
    {
        _availabilityRepository = availabilityRepository;
    }
    
    public async Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(Guid guestId)
    {
        return await Task.FromResult(Items.Where(b => b.GuestId == guestId).ToList());
    }

    public async Task<IEnumerable<Booking>> GetBookingsByPropertyIdAsync(Guid propertyId)
    {
        return await Task.FromResult(Items.Where(b => b.PropertyId == propertyId).ToList());
    }

    public async Task<bool> IsPropertyAvailableAsync(Guid propertyId, DateTime startDate, DateTime endDate)
    {
        // Check if there are confirmed bookings that overlap
        var overlappingBookings = Items
            .Where(b => b.PropertyId == propertyId &&
                        b.Status == BookingStatus.Confirmed &&
                        ((b.StartDate < endDate && b.EndDate > startDate))) // Overlapping condition
            .ToList();

        if (overlappingBookings.Count > 0)
        {
            return await Task.FromResult(false); // Property is booked
        }

        // Check if property has availability during the requested period
        var availableDates = await _availabilityRepository.GetAvailabilitiesByPropertyIdAsync(propertyId);

        bool hasAvailability = availableDates
            .Any(a => a.StartDate <= startDate && a.EndDate >= endDate); // Availability check

        return await Task.FromResult(hasAvailability);
    }
    
    public async Task<bool> ConfirmBookingAsync(Guid bookingId)
    {
        var booking = await GetByIdAsync(bookingId);
        if (booking == null || booking.Status != BookingStatus.Requested)
            return false; // Cannot confirm a non-existent or already processed booking.

        // Check if property is still available before confirming
        bool isAvailable = await IsPropertyAvailableAsync(booking.PropertyId, booking.StartDate, booking.EndDate);
        if (!isAvailable)
            return false;

        booking.Status = BookingStatus.Confirmed;
        await UpdateAsync(booking);

        // Mark the property as unavailable for the booked dates
        await _availabilityRepository.AddAsync(new Availability
        {
            PropertyId = booking.PropertyId,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate
        });

        return true;
    }

    public async Task<bool> CancelBookingAsync(Guid bookingId)
    {
        var booking = await GetByIdAsync(bookingId);
        if (booking == null || booking.Status != BookingStatus.Confirmed)
            return false; // Cannot cancel if not confirmed.

        booking.Status = BookingStatus.Cancelled;
        await UpdateAsync(booking);

        // Restore the availability for the cancelled dates
        var existingAvailability = (await _availabilityRepository.GetAvailabilitiesByPropertyIdAsync(booking.PropertyId))
            .FirstOrDefault(a => a.StartDate == booking.StartDate && a.EndDate == booking.EndDate);

        if (existingAvailability != null)
        {
            await _availabilityRepository.DeleteAsync(existingAvailability.Id);
        }

        return true;
    }
    
}
