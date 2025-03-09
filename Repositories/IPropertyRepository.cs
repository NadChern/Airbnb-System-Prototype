using AirbnbREST.DTOs;
using AirbnbREST.Models;
namespace AirbnbREST.Repositories;

public interface IPropertyRepository: IRepository<Property>
{
    Task<IEnumerable<Property>> SearchPropertiesAsync(PropertySearchDto searchParams);
    Task<IEnumerable<Property>> GetPropertiesByOwnerIdAsync(Guid ownerId);
}
