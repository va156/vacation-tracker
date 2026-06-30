using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Interfaces;

public interface IRequestRepository
{
    Task<IReadOnlyList<Request>> GetAllByEmployeeIdAsync(int employeeId);
    Task<IReadOnlyList<Request>> GetAllAsync();
    Task<Request?> GetByIdAsync(int id);
    Task<Request?> GetByIdWithDetailsAsync(int id);
    Task AddAsync(Request request);
    Task UpdateAsync(Request request);
}
