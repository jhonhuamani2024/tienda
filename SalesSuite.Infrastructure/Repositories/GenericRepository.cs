using Microsoft.EntityFrameworkCore;
using SalesSuite.Domain.Interfaces;
using SalesSuite.Infrastructure.Data;
using System.Linq.Expressions;

namespace SalesSuite.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation for CRUD operations
/// </summary>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly SalesDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(SalesDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Get all entities asynchronously
    /// </summary>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving all entities of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Get entity by ID asynchronously
    /// </summary>
    public async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            return await _dbSet.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving entity of type {typeof(T).Name} with ID {id}", ex);
        }
    }

    /// <summary>
    /// Find entities matching the predicate asynchronously
    /// </summary>
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error finding entities of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Get first entity matching the predicate asynchronously
    /// </summary>
    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error finding first entity of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Add entity asynchronously
    /// </summary>
    public async Task AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            await _dbSet.AddAsync(entity);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error adding entity of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Add multiple entities asynchronously
    /// </summary>
    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentException("Entities collection cannot be null or empty", nameof(entities));

        try
        {
            await _dbSet.AddRangeAsync(entities);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error adding multiple entities of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public void Update(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            _dbSet.Update(entity);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error updating entity of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Update multiple entities
    /// </summary>
    public void UpdateRange(IEnumerable<T> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentException("Entities collection cannot be null or empty", nameof(entities));

        try
        {
            _dbSet.UpdateRange(entities);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error updating multiple entities of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Delete entity asynchronously
    /// </summary>
    public async Task DeleteAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error deleting entity of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Delete entity by ID asynchronously
    /// </summary>
    public async Task DeleteByIdAsync(int id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error deleting entity of type {typeof(T).Name} with ID {id}", ex);
        }
    }

    /// <summary>
    /// Delete multiple entities asynchronously
    /// </summary>
    public async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentException("Entities collection cannot be null or empty", nameof(entities));

        try
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error deleting multiple entities of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Check if entity exists asynchronously
    /// </summary>
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await _dbSet.AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error checking existence of entity of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Count total entities asynchronously
    /// </summary>
    public async Task<int> CountAsync()
    {
        try
        {
            return await _dbSet.CountAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error counting entities of type {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Count entities matching predicate asynchronously
    /// </summary>
    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await _dbSet.CountAsync(predicate);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error counting entities of type {typeof(T).Name}", ex);
        }
    }
}
