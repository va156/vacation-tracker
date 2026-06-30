using Horizon.Server.Domain.ValueObjects;
using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;
using Horizon.Server.Modules.Employees.Domain.Entities;
using Horizon.Server.Modules.References.Domain.Entities;
using Horizon.Server.Modules.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.LeaveManagement.Domain.Entities;

/// <summary>
/// Represents a single leave period within a leave request.
/// One <see cref="Request"/> may include several <see cref="Leave"/> records
/// (e.g. two separate date ranges of the same leave type).
/// A leave may optionally reference a previous leave via <see cref="PreviousLeaveId"/>
/// to model reschedules or cancellations.
/// </summary>
[Index(nameof(EmployeeId))]
[Index(nameof(LeaveTypeId))]
[Index(nameof(StatusId))]
[Index(nameof(RequestId))]
public class Leave : BaseEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private Leave() { }

    /// <summary>
    /// Creates a new leave period.
    /// </summary>
    /// <param name="requestId">FK of the parent leave request.</param>
    /// <param name="employeeId">FK of the employee taking the leave.</param>
    /// <param name="leaveTypeId">FK of the leave type (e.g. annual, sick).</param>
    /// <param name="startDate">First day of the leave period (inclusive).</param>
    /// <param name="endDate">Last day of the leave period (inclusive).</param>
    /// <param name="durationDays">Pre-calculated calendar days for the period.</param>
    /// <param name="createdBy">ID of the user creating the record.</param>
    public Leave(int requestId, int employeeId, int leaveTypeId,
        DateTime startDate, DateTime endDate, decimal durationDays, int createdBy) : base(createdBy)
    {
        RequestId = requestId;
        EmployeeId = employeeId;
        LeaveTypeId = leaveTypeId;
        StartDate = startDate;
        EndDate = endDate;
        DurationDays = durationDays;
    }

    /// <summary>Gets the FK of the parent leave request.</summary>
    public int RequestId { get; private set; }

    /// <summary>Gets the FK of the employee taking the leave.</summary>
    public int EmployeeId { get; private set; }

    /// <summary>Gets the FK of the leave type.</summary>
    public int LeaveTypeId { get; private set; }

    /// <summary>Gets the FK of the current leave status.</summary>
    public int StatusId { get; private set; }

    /// <summary>Gets the first day of the leave period (inclusive).</summary>
    public DateTime StartDate { get; private set; }

    /// <summary>Gets the last day of the leave period (inclusive).</summary>
    public DateTime EndDate { get; private set; }

    /// <summary>Gets the total number of calendar days for this leave period.</summary>
    public decimal DurationDays { get; private set; }

    /// <summary>Gets the FK of the leave record that this one replaces (for reschedule/cancel), or <c>null</c>.</summary>
    public int? PreviousLeaveId { get; private set; }

    /// <summary>Gets an optional comment from the employee regarding this leave period.</summary>
    public string? Comment { get; private set; }

    /// <summary>Navigation property to the current leave status.</summary>
    public virtual LeaveStatus Status { get; private set; } = null!;

    /// <summary>Navigation property to the leave being replaced, or <c>null</c>.</summary>
    public virtual Leave? PreviousLeave { get; private set; }

    private readonly List<Leave> _childLeaves = new();
    /// <summary>Gets the leave records that replaced this one (reverse of <see cref="PreviousLeave"/>).</summary>
    public virtual IReadOnlyCollection<Leave> ChildLeaves => _childLeaves.AsReadOnly();
}
