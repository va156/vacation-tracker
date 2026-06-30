using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Interfaces;

/// <summary>
/// Data-access contract for <see cref="Request"/> aggregates.
/// All queries automatically filter out soft-deleted records (<c>IsActive = false</c>).
/// </summary>
public interface IRequestRepository
{
    /// <summary>
    /// Returns all active requests belonging to the specified employee,
    /// ordered by creation date descending.
    /// </summary>
    Task<IReadOnlyList<Request>> GetAllByEmployeeIdAsync(int employeeId);

    /// <summary>Returns all active requests across all employees, ordered by creation date descending.</summary>
    Task<IReadOnlyList<Request>> GetAllAsync();

    /// <summary>Returns the active request with the given ID (without navigation properties), or <c>null</c>.</summary>
    Task<Request?> GetByIdAsync(int id);

    /// <summary>
    /// Returns the active request with the given ID, eagerly loading status, template,
    /// leaves, and approval history, or <c>null</c>.
    /// </summary>
    Task<Request?> GetByIdWithDetailsAsync(int id);

    /// <summary>Adds a new request to the repository (call <c>SaveChangesAsync</c> to persist).</summary>
    Task AddAsync(Request request);

    /// <summary>Marks the request as modified in the change tracker (call <c>SaveChangesAsync</c> to persist).</summary>
    Task UpdateAsync(Request request);
}
