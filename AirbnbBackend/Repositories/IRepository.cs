using System.Linq.Expressions;
namespace AirbnbREST.Repositories;

// Interface for all storages (makes RESP API storage-agnostic)
// Standard CRUD + find
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
