using System.Linq.Expressions;
using AirbnbREST.Data;
using AirbnbREST.Models;
using Microsoft.EntityFrameworkCore;
namespace AirbnbREST.Repositories;

public class AvailabilityRepository : IAvailabilityRepository
{
    private readonly ApplicationDbContext _context;

    public AvailabilityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Availability>> GetAllAsync()
    {
        return await _context.Availabilities.ToListAsync();
    }

    public async Task<Availability?> GetByIdAsync(Guid id)
    {
        return await _context.Availabilities.FindAsync(id);
    }

    public async Task<IEnumerable<Availability>> GetAvailabilitiesByPropertyIdAsync(Guid propertyId)
    {
        return await _context.Availabilities
            .Where(a => a.PropertyId == propertyId)
            .ToListAsync();
    }

    public async Task AddAsync(Availability availability)
    {
        await _context.Availabilities.AddAsync(availability);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Availability availability)
    {
        _context.Availabilities.Update(availability);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var availability = await _context.Availabilities.FindAsync(id);
        if (availability != null)
        {
            _context.Availabilities.Remove(availability);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Availability>> FindAsync(Expression<Func<Availability, bool>> predicate)
    {
        return await _context.Availabilities.Where(predicate).ToListAsync();
    }
}
