using Horizon.Server.Modules.ApprovalWorkflow.Domain.Events;
using Horizon.Server.Modules.Employees.Domain.Entities;
using Horizon.Server.Modules.LeaveManagement.Domain.Entities;
using Horizon.Server.Modules.References.Domain.Entities;
using Horizon.Server.Modules.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

/// <summary>
/// Aggregate root that represents a leave request submitted by an employee.
/// A request groups one or more <see cref="Leave"/> records (individual leave periods)
/// and moves through an approval workflow defined by an <see cref="ApprovalTemplate"/>.
/// The current position in the workflow is tracked by <see cref="CurrentStageNumber"/>.
/// </summary>
[Index(nameof(RequestNumber), IsUnique = true)]
[Index(nameof(EmployeeId))]
[Index(nameof(StatusId))]
public class Request : BaseEntity, IAggregateRoot
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private Request() { }

    /// <summary>
    /// Creates a new leave request in draft state.
    /// </summary>
    /// <param name="requestNumber">Human-readable unique identifier (e.g. "REQ-2026-0001").</param>
    /// <param name="operationTypeId">FK of the operation type (e.g. new leave, reschedule).</param>
    /// <param name="employeeId">FK of the employee submitting the request.</param>
    /// <param name="departmentId">FK of the employee's department at time of submission.</param>
    /// <param name="approvalTemplateId">FK of the workflow template to follow.</param>
    /// <param name="createdBy">ID of the user creating the request.</param>
    public Request(string requestNumber, int operationTypeId, int employeeId,
            int departmentId, int approvalTemplateId, int createdBy) : base(createdBy)
    {
        RequestNumber = requestNumber;
        OperationTypeId = operationTypeId;
        EmployeeId = employeeId;
        DepartmentId = departmentId;
        ApprovalTemplateId = approvalTemplateId;
        _leaves = new List<Leave>();
    }

    /// <summary>Gets the human-readable unique request number.</summary>
    public string RequestNumber { get; private set; } = null!;

    /// <summary>Gets the FK of the operation type (e.g. NEW_LEAVE, RESCHEDULE).</summary>
    public int OperationTypeId { get; private set; }

    /// <summary>Gets the FK of the current request status.</summary>
    public int StatusId { get; private set; }

    /// <summary>Gets the FK of the employee who owns this request.</summary>
    public int EmployeeId { get; private set; }

    /// <summary>Gets the FK of the department the request was submitted for.</summary>
    public int DepartmentId { get; private set; }

    /// <summary>Gets the FK of the approval template governing the workflow.</summary>
    public int ApprovalTemplateId { get; private set; }

    /// <summary>Gets the optional free-text comment from the submitting employee.</summary>
    public string? Comment { get; private set; }

    /// <summary>
    /// Gets the stage number currently awaiting approval, or <c>null</c> if the request
    /// has not yet been submitted or has reached a terminal state.
    /// </summary>
    public int? CurrentStageNumber { get; private set; }

    /// <summary>Gets the UTC timestamp when the request was submitted for approval, or <c>null</c>.</summary>
    public DateTime? SubmittedAt { get; private set; }

    /// <summary>Gets the UTC timestamp when the request reached a terminal status, or <c>null</c>.</summary>
    public DateTime? CompletedAt { get; private set; }

    /// <summary>Navigation property to the current status.</summary>
    public virtual RequestStatus Status { get; private set; } = null!;

    /// <summary>Navigation property to the approval template.</summary>
    public virtual ApprovalTemplate ApprovalTemplate { get; private set; } = null!;

    private readonly List<Leave> _leaves;
    /// <summary>Gets the individual leave periods included in this request.</summary>
    public virtual IReadOnlyCollection<Leave> Leaves => _leaves.AsReadOnly();

    private readonly List<ApprovalHistory> _approvalHistory = new();
    /// <summary>Gets the chronological log of approver decisions for this request.</summary>
    public virtual IReadOnlyCollection<ApprovalHistory> ApprovalHistory => _approvalHistory.AsReadOnly();

    /// <summary>
    /// Marks the request as submitted and raises a <see cref="RequestSubmittedEvent"/>
    /// domain event so that downstream handlers (e.g. notifications, balance reservation)
    /// can react without tight coupling.
    /// </summary>
    public void Submit()
    {
        SubmittedAt = DateTime.UtcNow;
        CurrentStageNumber = 1;
        AddDomainEvent(new RequestSubmittedEvent(Id, EmployeeId, RequestNumber));
    }

    /// <summary>
    /// Records an approval decision, advances the stage, and raises the appropriate domain event.
    /// If <paramref name="isFinalApproval"/> is <c>true</c> the workflow is considered complete.
    /// </summary>
    /// <param name="newStatusId">StatusId to assign (PENDING_HR or APPROVED).</param>
    /// <param name="nextStageNumber">Next stage to wait on, or <c>null</c> when fully approved.</param>
    /// <param name="isFinalApproval"><c>true</c> when all stages have been approved.</param>
    public void Approve(int newStatusId, int? nextStageNumber, bool isFinalApproval)
    {
        StatusId = newStatusId;
        CurrentStageNumber = nextStageNumber;
        if (isFinalApproval)
        {
            CompletedAt = DateTime.UtcNow;
            AddDomainEvent(new RequestApprovedEvent(Id, EmployeeId, RequestNumber));
        }
    }

    /// <summary>
    /// Records a rejection, sets the terminal status and raises <see cref="RequestRejectedEvent"/>.
    /// </summary>
    /// <param name="rejectedStatusId">StatusId that represents REJECTED.</param>
    /// <param name="reason">Optional reason supplied by the approver.</param>
    public void Reject(int rejectedStatusId, string? reason)
    {
        StatusId = rejectedStatusId;
        CurrentStageNumber = null;
        CompletedAt = DateTime.UtcNow;
        AddDomainEvent(new RequestRejectedEvent(Id, EmployeeId, RequestNumber, reason));
    }

    /// <summary>
    /// Returns the request to the employee for revision, sets SENT_BACK status
    /// and raises <see cref="RequestSentBackEvent"/>.
    /// </summary>
    /// <param name="sentBackStatusId">StatusId that represents SENT_BACK.</param>
    /// <param name="comment">Comment explaining what needs to be revised.</param>
    public void SendBack(int sentBackStatusId, string? comment)
    {
        StatusId = sentBackStatusId;
        CurrentStageNumber = null;
        CompletedAt = DateTime.UtcNow;
        AddDomainEvent(new RequestSentBackEvent(Id, EmployeeId, RequestNumber, comment));
    }
}
