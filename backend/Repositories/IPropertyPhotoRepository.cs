using AirbnbREST.Models;

namespace AirbnbREST.Repositories;

public interface IPropertyPhotoRepository : IRepository<PropertyPhoto>
{
    Task<IEnumerable<PropertyPhoto>> GetPhotosByPropertyIdAsync(Guid propertyId);
}

