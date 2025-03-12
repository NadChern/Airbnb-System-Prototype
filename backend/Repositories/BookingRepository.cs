using System.Linq.Expressions;
using AirbnbREST.Data;
using AirbnbREST.Models;
using Microsoft.EntityFrameworkCore;

namespace AirbnbREST.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings.FindAsync(id);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(Guid guestId)
    {
        return await _context.Bookings.Where(b => b.GuestId == guestId).ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByPropertyIdAsync(Guid propertyId)
    {
        return await _context.Bookings.Where(b => b.PropertyId == propertyId).ToListAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<bool> IsPropertyAvailableAsync(Guid propertyId,
        DateTime startDate, DateTime endDate)
    {
        var overlappingBookings = await _context.Bookings
            .Where(b => b.PropertyId == propertyId &&
                        b.Status == BookingStatus.Confirmed &&
                        b.StartDate < endDate && b.EndDate > startDate)
            .ToListAsync();

        return !overlappingBookings.Any();
    }

    public async Task<bool> ConfirmBookingAsync(Guid bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null || booking.Status != BookingStatus.Requested)
            return false;

        bool isAvailable = await IsPropertyAvailableAsync(booking.PropertyId, booking.StartDate, booking.EndDate);
        if (!isAvailable)
            return false;

        booking.Status = BookingStatus.Confirmed;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelBookingAsync(Guid bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null || booking.Status != BookingStatus.Confirmed)
            return false;

        booking.Status = BookingStatus.Cancelled;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<IEnumerable<Booking>> FindAsync(Expression<Func<Booking, bool>> predicate)
    {
        return await _context.Bookings.Where(predicate).ToListAsync();
    }
}
