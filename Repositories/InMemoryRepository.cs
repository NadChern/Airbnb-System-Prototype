using System.Linq.Expressions;
namespace AirbnbREST.Repositories;

// In-memory storage for testing (since PostgreSQL is not ready)
public class InMemoryRepository<T> : IRepository<T> where T : class
{
    protected readonly List<T> Items = new List<T>();

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Task.FromResult(Items);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Task.FromResult(Items.AsQueryable().Where(predicate).ToList());
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var item = Items.FirstOrDefault(e => e?.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) == true);
        return await Task.FromResult(item);
    }

    public async Task AddAsync(T entity)
    {
        Items.Add(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(T entity)
    {
        var existing = await GetByIdAsync((Guid)entity.GetType().GetProperty("Id")?.GetValue(entity)!);
        if (existing != null)
        {
            Items.Remove(existing);
            Items.Add(entity);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            Items.Remove(entity);
        }
    }
}
