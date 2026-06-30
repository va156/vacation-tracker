using Horizon.Server.Modules.LeaveManagement.Domain.Entities;

namespace Horizon.Server.Modules.LeaveManagement.Domain.Interfaces;

/// <summary>
/// Data-access contract for <see cref="LeaveBalance"/> records.
/// All queries automatically exclude soft-deleted records.
/// </summary>
public interface ILeaveBalanceRepository
{
    /// <summary>
    /// Returns all active balances for the specified employee, optionally filtered to a single year.
    /// Results are ordered by leave type name.
    /// </summary>
    /// <param name="employeeId">FK of the employee.</param>
    /// <param name="year">Calendar year to filter by, or <c>null</c> to return all years.</param>
    Task<IReadOnlyList<LeaveBalance>> GetByEmployeeIdAsync(int employeeId, int? year = null);

    /// <summary>Returns all active balances for every employee in the specified calendar year.</summary>
    Task<IReadOnlyList<LeaveBalance>> GetAllForYearAsync(int year);

    /// <summary>Returns the active balance with the given primary key, or <c>null</c>.</summary>
    Task<LeaveBalance?> GetByIdAsync(int id);

    /// <summary>Adds a new balance record (call <c>SaveChangesAsync</c> to persist).</summary>
    Task AddAsync(LeaveBalance balance);

    /// <summary>Marks a balance record as modified in the change tracker (call <c>SaveChangesAsync</c> to persist).</summary>
    Task UpdateAsync(LeaveBalance balance);
}
