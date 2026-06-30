using Horizon.Server.Modules.Employees.Domain.Entities;
using Horizon.Server.Modules.References.Domain.Entities;
using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.LeaveManagement.Domain.Entities;

/// <summary>
/// Tracks an employee's leave quota for a specific <see cref="LeaveType"/> and calendar year.
/// <see cref="Available"/> is calculated in real time from the other fields; it is not stored.
/// Balances are adjusted by balance transactions created when requests are approved.
/// </summary>
public class LeaveBalance : BaseEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private LeaveBalance() { }

    /// <summary>
    /// Creates a new leave balance record for an employee.
    /// </summary>
    /// <param name="employeeId">FK of the employee this balance belongs to.</param>
    /// <param name="leaveTypeId">FK of the leave type.</param>
    /// <param name="year">The calendar year this balance covers.</param>
    /// <param name="calculatedAt">UTC timestamp when the balance was last recalculated.</param>
    /// <param name="createdBy">ID of the user (or system) creating the record.</param>
    public LeaveBalance(int employeeId, int leaveTypeId, int year, DateTime calculatedAt, int createdBy) : base(createdBy)
    {
        EmployeeId = employeeId;
        LeaveTypeId = leaveTypeId;
        Year = year;
        CalculatedAt = calculatedAt;
    }

    /// <summary>Gets the FK of the employee this balance belongs to.</summary>
    public int EmployeeId { get; private set; }

    /// <summary>Gets the FK of the leave type (e.g. annual, sick).</summary>
    public int LeaveTypeId { get; private set; }

    /// <summary>Gets the calendar year this balance covers.</summary>
    public int Year { get; private set; }

    /// <summary>Gets the total number of days the employee is entitled to for this leave type in the year.</summary>
    public decimal Entitled { get; private set; }

    /// <summary>Gets the number of days already taken (approved and completed leaves).</summary>
    public decimal Used { get; private set; }

    /// <summary>Gets the number of days reserved by approved-but-not-yet-started leaves.</summary>
    public decimal Planned { get; private set; }

    /// <summary>Gets the remaining available days: <c>Entitled - Used - Planned</c>.</summary>
    public decimal Available => Entitled - Used - Planned;

    /// <summary>Gets the UTC timestamp of the most recent balance recalculation.</summary>
    public DateTime CalculatedAt { get; private set; }

    /// <summary>Navigation property to the owning employee.</summary>
    public virtual Employee Employee { get; private set; } = null!;

    /// <summary>Navigation property to the leave type.</summary>
    public virtual LeaveType LeaveType { get; private set; } = null!;
}
