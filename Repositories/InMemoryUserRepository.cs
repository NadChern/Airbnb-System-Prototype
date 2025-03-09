using AirbnbREST.Models;

namespace AirbnbREST.Repositories;

// In-memory user repo implementation
public class InMemoryUserRepository : InMemoryRepository<User>, IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Task.FromResult(Items.FirstOrDefault(u => 
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
    }

    public async Task<IEnumerable<User>> FindByNameAsync(string firstName,
        string lastName)
    {
        return await Task.FromResult(Items
            .Where(u => u.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                        u.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
            .ToList());
    }

    public async Task<UserRole?> GetUserRoleAsync(Guid userId)
    {
        var user = Items.FirstOrDefault(u => u.Id == userId);
        return await Task.FromResult(user?.Role);
    }
}
