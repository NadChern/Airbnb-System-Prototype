using AirbnbREST.DTOs;
using AirbnbREST.Models;

namespace AirbnbREST.Repositories;

public class InMemoryPropertyRepository : InMemoryRepository<Property>, IPropertyRepository
{
    private readonly IAvailabilityRepository _availabilityRepository;
    private readonly IBookingRepository _bookingRepository;

    public InMemoryPropertyRepository(IAvailabilityRepository availabilityRepository,
        IBookingRepository bookingRepository)
    {
        _availabilityRepository = availabilityRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<Property>> SearchPropertiesAsync(PropertySearchDto searchParams)
    {
        var query = Items.AsQueryable();

        if (!string.IsNullOrEmpty(searchParams.Title))
            query = query.Where(p => p.Title.Contains(searchParams.Title, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(searchParams.City))
            query = query.Where(p => p.City.Equals(searchParams.City, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(searchParams.State))
            query = query.Where(p => p.State.Equals(searchParams.State, StringComparison.OrdinalIgnoreCase));
        
        if (searchParams.MinBedrooms.HasValue)
            query = query.Where(p => p.Bedrooms >= searchParams.MinBedrooms.Value);

        if (searchParams.MaxBedrooms.HasValue)
            query = query.Where(p => p.Bedrooms <= searchParams.MaxBedrooms.Value);

        if (searchParams.MinBathrooms.HasValue)
            query = query.Where(p => p.Bathrooms >= searchParams.MinBathrooms.Value);

        if (searchParams.MaxBathrooms.HasValue)
            query = query.Where(p => p.Bathrooms <= searchParams.MaxBathrooms.Value);

        if (searchParams.MinPrice.HasValue)
            query = query.Where(p => p.PricePerNight >= searchParams.MinPrice.Value);

        if (searchParams.MaxPrice.HasValue)
            query = query.Where(p => p.PricePerNight <= searchParams.MaxPrice.Value);

        // Check property availability if dates are provided
        if (searchParams.StartDate.HasValue && searchParams.EndDate.HasValue)
        {
            var availablePropertyIds = (await _availabilityRepository.GetAvailabilitiesByPropertyIdAsync(Guid.Empty))
                .AsQueryable()
                .Where(a => a.StartDate <= searchParams.StartDate.Value && a.EndDate >= searchParams.EndDate.Value)
                .Select(a => a.PropertyId)
                .ToList();

            var bookedPropertyIds = (await _bookingRepository.GetAllAsync())
                .AsQueryable()
                .Where(b => b.Status == BookingStatus.Confirmed &&
                            (b.StartDate < searchParams.EndDate.Value && b.EndDate > searchParams.StartDate.Value))
                .Select(b => b.PropertyId)
                .ToList();

            // filter out booked properties
            query = query.Where(p => availablePropertyIds.Contains(p.Id) && !bookedPropertyIds.Contains(p.Id));
        }

        return await Task.FromResult(query.ToList());
    }

    public async Task<IEnumerable<Property>> GetPropertiesByOwnerIdAsync(Guid ownerId)
    {
        return await Task.FromResult(Items.Where(p => p.Owner == ownerId).ToList());
    }
}
