using Horizon.Server.Modules.Shared.Application.Interfaces;
using Horizon.Server.Modules.Shared.Domain.Abstractions;
using Horizon.Server.Modules.Shared.Domain.Common;
using MediatR;

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
    private readonly IPublisher _publisher;
    private readonly ILogger<UnitOfWork> _logger;
    private bool _disposed;

    /// <summary>
    /// Initialises a new <see cref="UnitOfWork"/> with the given context, MediatR publisher and logger.
    /// </summary>
    /// <param name="context">The shared EF Core database context (scoped).</param>
    /// <param name="publisher">MediatR publisher used to dispatch domain events after save.</param>
    /// <param name="logger">Logger used to report persistence failures.</param>
    public UnitOfWork(AppDbContext context, IPublisher publisher, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _publisher = publisher;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Collect domain events from all tracked aggregates before saving.
            var domainEvents = _context.ChangeTracker
                .Entries<BaseEntity>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            // Persist changes first so that event handlers can safely query the DB.
            var result = await _context.SaveChangesAsync(cancellationToken);

            // Clear events from entities BEFORE dispatching so re-entrant saves don't re-fire them.
            foreach (var entry in _context.ChangeTracker.Entries<BaseEntity>())
                entry.Entity.ClearDomainEvents();

            // Dispatch each event through the MediatR pipeline (Observer / Domain Events pattern).
            foreach (var domainEvent in domainEvents)
            {
                _logger.LogDebug("Dispatching domain event {EventType} via MediatR", domainEvent.GetType().Name);
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
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
