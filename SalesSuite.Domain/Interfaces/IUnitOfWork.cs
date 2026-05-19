namespace SalesSuite.Domain.Interfaces;

/// <summary>
/// Unit of Work pattern for managing repositories and transactions
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// Get repository for entity type
    /// </summary>
    IGenericRepository<T> Repository<T>() where T : class;

    /// <summary>
    /// Save changes asynchronously with automatic transaction
    /// </summary>
    Task<int> CommitAsync();

    /// <summary>
    /// Rollback changes asynchronously
    /// </summary>
    Task RollbackAsync();

    /// <summary>
    /// Begin a transaction asynchronously
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commit transaction asynchronously
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rollback transaction asynchronously
    /// </summary>
    Task RollbackTransactionAsync();
}
