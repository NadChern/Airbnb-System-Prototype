using System.Linq.Expressions;
using AirbnbREST.Data;
using AirbnbREST.Models;
namespace AirbnbREST.Repositories;
using Microsoft.EntityFrameworkCore;

public class PropertyPhotoRepository : IPropertyPhotoRepository
{
    private readonly ApplicationDbContext _context;

    public PropertyPhotoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PropertyPhoto>> GetAllAsync()
    {
        return await _context.PropertyPhotos.ToListAsync();
    }

    public async Task<PropertyPhoto?> GetByIdAsync(Guid id)
    {
        return await _context.PropertyPhotos.FindAsync(id);
    }

    public async Task<IEnumerable<PropertyPhoto>> GetPhotosByPropertyIdAsync(Guid propertyId)
    {
        return await _context.PropertyPhotos.Where(p => p.PropertyId == propertyId).ToListAsync();
    }

    public async Task AddAsync(PropertyPhoto photo)
    {
        await _context.PropertyPhotos.AddAsync(photo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PropertyPhoto photo)
    {
        _context.PropertyPhotos.Update(photo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var photo = await _context.PropertyPhotos.FindAsync(id);
        if (photo != null)
        {
            _context.PropertyPhotos.Remove(photo);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<PropertyPhoto>> FindAsync(Expression<Func<PropertyPhoto, bool>> predicate)
    {
        return await _context.PropertyPhotos.Where(predicate).ToListAsync();
    }
}
