using System.Linq.Expressions;
using AirbnbREST.Data;
using AirbnbREST.Models;
using Microsoft.EntityFrameworkCore;

namespace AirbnbREST.Repositories;

// User repo implementation
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> FindByNameAsync(string firstName, string lastName)
    {
        return await _context.Users
            .Where(u => u.FirstName == firstName && u.LastName == lastName)
            .ToListAsync();
    }

    public async Task<UserRole?> GetUserRoleAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user?.Role;
    }
}
