using Microsoft.EntityFrameworkCore.Storage;
using SalesSuite.Domain.Interfaces;
using SalesSuite.Infrastructure.Data;
using System.Collections;

namespace SalesSuite.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing repositories and transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly SalesDbContext _context;
    private IDbContextTransaction? _transaction;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(SalesDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get or create repository for entity type
    /// </summary>
    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>).MakeGenericType(type);
            var repositoryInstance = Activator.CreateInstance(repositoryType, _context);
            _repositories.Add(type, repositoryInstance!);
        }

        return (IGenericRepository<T>)_repositories[type];
    }

    /// <summary>
    /// Save changes asynchronously with automatic transaction
    /// </summary>
    public async Task<int> CommitAsync()
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Error committing changes. Transaction has been rolled back.", ex);
        }
    }

    /// <summary>
    /// Rollback changes asynchronously
    /// </summary>
    public async Task RollbackAsync()
    {
        try
        {
            await _context.ChangeTracker.Entries()
                .ForEach(entry => entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error rolling back changes.", ex);
        }
    }

    /// <summary>
    /// Begin a transaction asynchronously
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commit transaction asynchronously
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch (Exception ex)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
            throw new InvalidOperationException("Error committing transaction.", ex);
        }
    }

    /// <summary>
    /// Rollback transaction asynchronously
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error rolling back transaction.", ex);
        }
    }

    /// <summary>
    /// Dispose resources
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }

        await _context.DisposeAsync();
    }
}
