using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Services
{
    /// <summary>
    /// Thread-safe generator of unique, human-readable leave-request numbers.
    /// Format: <c>REQ-{YYYY}-{NNNNN}</c> (e.g. "REQ-2026-00001").
    /// Numbers are sequential per calendar year and zero-padded to five digits so that
    /// alphabetical and numerical sort order always match.
    /// </summary>
    /// <remarks>
    /// Registered as a <b>Singleton</b> in DI so that the <see cref="SemaphoreSlim"/>
    /// instance lives for the full application lifetime and provides genuine mutual
    /// exclusion across concurrent HTTP requests.
    /// </remarks>
    public class RequestNumberGenerator
    {
        private readonly IServiceScopeFactory _scopeFactory;

        // SYNC: SemaphoreSlim(1,1) — prevents concurrent requests from receiving
        // the same sequence number when two threads read MAX(Id) simultaneously.
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>Initialises the generator with a scope factory (required because the
        /// class is Singleton but <see cref="AppDbContext"/> is Scoped).</summary>
        public RequestNumberGenerator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Atomically determines the next sequence number for the given year and returns
        /// a formatted request number string.
        /// </summary>
        /// <param name="year">The calendar year for which to generate the number.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the semaphore.</param>
        /// <returns>A string such as <c>"REQ-2026-00001"</c>.</returns>
        public async Task<string> GenerateAsync(int year, CancellationToken cancellationToken = default)
        {
            // SYNC: acquire the semaphore — only one thread enters this critical section at a time.
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Find the highest sequence number already used for this year.
                var prefix = $"REQ-{year}-";
                var maxNumber = await db.Set<Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities.Request>()
                    .Where(r => r.RequestNumber.StartsWith(prefix))
                    .Select(r => r.RequestNumber)
                    .ToListAsync(cancellationToken);

                var nextSeq = maxNumber
                    .Select(n =>
                    {
                        var parts = n.Split('-');
                        return parts.Length == 3 && int.TryParse(parts[2], out var seq) ? seq : 0;
                    })
                    .DefaultIfEmpty(0)
                    .Max() + 1;

                return $"REQ-{year}-{nextSeq:D5}";
            }
            finally
            {
                // SYNC: release the semaphore so the next waiting thread can proceed.
                _semaphore.Release();
            }
        }
    }
}
