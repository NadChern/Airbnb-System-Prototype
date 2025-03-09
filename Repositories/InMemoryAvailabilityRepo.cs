using AirbnbREST.Models;

namespace AirbnbREST.Repositories;

public class InMemoryAvailabilityRepo: InMemoryRepository<Availability>, IAvailabilityRepository
{
    public async Task<IEnumerable<Availability>> GetAvailabilitiesByPropertyIdAsync(Guid propertyId)
    {
        return await Task.FromResult(Items.Where(a => a.PropertyId == propertyId).ToList());
    }
}
