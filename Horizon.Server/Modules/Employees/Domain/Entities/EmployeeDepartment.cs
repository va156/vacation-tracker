using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Employees.Domain.Entities;

/// <summary>
/// Join entity that records an employee's assignment to a department in a specific position.
/// Supports temporal tracking via <see cref="StartDate"/> and <see cref="EndDate"/>,
/// and partial assignments via <see cref="FTE"/> (full-time equivalent).
/// An employee may have multiple assignments, but exactly one should have <see cref="IsPrimary"/>
/// set to <c>true</c> at any given time.
/// </summary>
public class EmployeeDepartment : BaseEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private EmployeeDepartment() { }

    /// <summary>
    /// Creates a new department assignment record.
    /// </summary>
    /// <param name="employeeId">FK of the assigned employee.</param>
    /// <param name="departmentId">FK of the target department.</param>
    /// <param name="positionId">FK of the position held in this department.</param>
    /// <param name="startDate">Date the assignment became effective.</param>
    /// <param name="isPrimary"><c>true</c> if this is the employee's primary department.</param>
    /// <param name="fte">Full-time equivalent (1.0 = full-time, 0.5 = half-time).</param>
    /// <param name="createdBy">ID of the HR user creating the assignment.</param>
    public EmployeeDepartment(int employeeId, int departmentId, int positionId,
        DateTime startDate, bool isPrimary, decimal fte, int createdBy) : base(createdBy)
    {
        EmployeeId = employeeId;
        DepartmentId = departmentId;
        PositionId = positionId;
        StartDate = startDate;
        IsPrimary = isPrimary;
        FTE = fte;
    }

    /// <summary>Gets the FK of the assigned employee.</summary>
    public int EmployeeId { get; private set; }

    /// <summary>Gets the FK of the department.</summary>
    public int DepartmentId { get; private set; }

    /// <summary>Gets the FK of the position held in this department.</summary>
    public int PositionId { get; private set; }

    /// <summary>Gets the date the assignment became effective.</summary>
    public DateTime StartDate { get; private set; }

    /// <summary>Gets the date the assignment ended, or <c>null</c> if still active.</summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>Gets <c>true</c> when this is the employee's primary department assignment.</summary>
    public bool IsPrimary { get; private set; }

    /// <summary>Gets the full-time equivalent (1.0 = full-time, 0.5 = part-time, etc.).</summary>
    public decimal FTE { get; private set; }

    /// <summary>Navigation property to the employee.</summary>
    public virtual Employee Employee { get; private set; } = null!;

    /// <summary>Navigation property to the department.</summary>
    public virtual Department Department { get; private set; } = null!;

    /// <summary>Navigation property to the position.</summary>
    public virtual Position Position { get; private set; } = null!;
}
