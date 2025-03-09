using AirbnbREST.Models;

namespace AirbnbREST.Repositories;

public interface IAvailabilityRepository: IRepository<Availability>
{
    Task<IEnumerable<Availability>> GetAvailabilitiesByPropertyIdAsync(Guid propertyId);
}
