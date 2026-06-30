using Horizon.Server.Modules.LeaveManagement.Domain.Entities;

namespace Horizon.Server.Modules.LeaveManagement.Domain.Interfaces;

public interface ILeaveBalanceRepository
{
    Task<IReadOnlyList<LeaveBalance>> GetByEmployeeIdAsync(int employeeId, int? year = null);
    Task<IReadOnlyList<LeaveBalance>> GetAllForYearAsync(int year);
    Task<LeaveBalance?> GetByIdAsync(int id);
    Task AddAsync(LeaveBalance balance);
    Task UpdateAsync(LeaveBalance balance);
}
