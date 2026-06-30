using Horizon.Server.Modules.Shared.Application.Interfaces;
using Horizon.Server.Modules.Shared.Domain.Abstractions;

namespace Horizon.Server.Modules.Shared.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private bool _disposed;

    public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при сохранении изменений в базу данных");
            throw;
        }
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken) > 0;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        // DbContext is owned and disposed by the DI container (scoped lifetime).
        // Disposing it here would cause double-dispose; we only suppress finalization.
        GC.SuppressFinalize(this);
    }
}