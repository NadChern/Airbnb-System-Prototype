using AirbnbREST.Models;
namespace AirbnbREST.Repositories;

// Need to specify later here what specific queries needed from me besides
// already provided
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email); // for auth, user login
    Task<IEnumerable<User>> FindByNameAsync(string firstName, string lastName);
    Task<UserRole?> GetUserRoleAsync(Guid userId);
}
