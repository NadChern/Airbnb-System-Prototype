using System.Linq.Expressions;
using AirbnbREST.Data;
using AirbnbREST.DTOs;
using AirbnbREST.Models;
using Microsoft.EntityFrameworkCore;

namespace AirbnbREST.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly ApplicationDbContext _context;

    public PropertyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _context.Properties.ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(Guid id)
    {
        return await _context.Properties.FindAsync(id);
    }

    public async Task<IEnumerable<Property>> FindAsync(Expression<Func<Property, bool>> predicate)
    {
        return await _context.Properties.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(Property entity)
    {
        await _context.Properties.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Property entity)
    {
        _context.Properties.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var property = await _context.Properties.FindAsync(id);
        if (property != null)
        {
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Property>> GetPropertiesByOwnerIdAsync(Guid ownerId)
    {
        return await _context.Properties
            .Where(p => p.Owner == ownerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> SearchPropertiesAsync(PropertySearchDto searchParams)
    {
        var query = _context.Properties.AsQueryable();

        if (!string.IsNullOrEmpty(searchParams.Title))
            query = query.Where(p => p.Title.Contains(searchParams.Title));

        if (!string.IsNullOrEmpty(searchParams.City))
            query = query.Where(p => p.City.Equals(searchParams.City));

        if (!string.IsNullOrEmpty(searchParams.State))
            query = query.Where(p => p.State.Equals(searchParams.State));

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

        return await query.ToListAsync();
    }
}
