using Horizon.Server.Modules.Shared.Application.Interfaces;
using Horizon.Server.Modules.Shared.Domain.Abstractions;

namespace Horizon.Server.Modules.Shared.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IUnitOfWork"/>.
/// Wraps <see cref="AppDbContext"/> to coordinate saves and database transactions.
/// The context itself is owned by the DI container (scoped lifetime) and must not
/// be disposed here to avoid double-dispose issues.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private bool _disposed;

    /// <summary>
    /// Initialises a new <see cref="UnitOfWork"/> with the given context and logger.
    /// </summary>
    /// <param name="context">The shared EF Core database context (scoped).</param>
    /// <param name="logger">Logger used to report persistence failures.</param>
    public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving changes to the database");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken) > 0;
    }

    /// <inheritdoc />
    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await action();
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await action();
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Signals that this instance is no longer needed.
    /// The underlying <see cref="AppDbContext"/> is managed by the DI container and
    /// is intentionally NOT disposed here to prevent double-dispose.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        // DbContext is owned and disposed by the DI container (scoped lifetime).
        // Disposing it here would cause double-dispose; we only suppress finalization.
        GC.SuppressFinalize(this);
    }
}
