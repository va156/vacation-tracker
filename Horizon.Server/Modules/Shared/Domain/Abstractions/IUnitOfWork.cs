namespace Horizon.Server.Modules.Shared.Domain.Abstractions;

/// <summary>
/// Coordinates writing to the database within a single business operation.
/// Implementations wrap an EF Core <c>DbContext</c> and expose transaction helpers so
/// that multiple repository calls can be committed atomically.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists all pending EF Core change tracker entries.
    /// </summary>
    /// <param name="cancellationToken">Token to observe for cancellation.</param>
    /// <returns>The number of database rows affected.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists all pending changes and returns <c>true</c> when at least one row was affected.
    /// </summary>
    /// <param name="cancellationToken">Token to observe for cancellation.</param>
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes <paramref name="action"/> inside a database transaction and returns its result.
    /// The transaction is committed on success, or rolled back on any exception.
    /// </summary>
    /// <typeparam name="T">The type returned by <paramref name="action"/>.</typeparam>
    /// <param name="action">The work to perform atomically.</param>
    /// <param name="cancellationToken">Token to observe for cancellation.</param>
    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes <paramref name="action"/> inside a database transaction (no return value).
    /// The transaction is committed on success, or rolled back on any exception.
    /// </summary>
    /// <param name="action">The work to perform atomically.</param>
    /// <param name="cancellationToken">Token to observe for cancellation.</param>
    Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
}
