using System.Linq.Expressions;

namespace SalesSuite.Domain.Interfaces;

/// <summary>
/// Interface for generic CRUD operations on entities
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Get all entities asynchronously
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Get entity by ID asynchronously
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Find entities matching the predicate asynchronously
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Get first entity matching the predicate asynchronously
    /// </summary>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Add entity asynchronously
    /// </summary>
    Task AddAsync(T entity);

    /// <summary>
    /// Add multiple entities asynchronously
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Update entity
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Update multiple entities
    /// </summary>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Delete entity asynchronously
    /// </summary>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Delete entity by ID asynchronously
    /// </summary>
    Task DeleteByIdAsync(int id);

    /// <summary>
    /// Delete multiple entities asynchronously
    /// </summary>
    Task DeleteRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Check if entity exists asynchronously
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Count total entities asynchronously
    /// </summary>
    Task<int> CountAsync();

    /// <summary>
    /// Count entities matching predicate asynchronously
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
}
