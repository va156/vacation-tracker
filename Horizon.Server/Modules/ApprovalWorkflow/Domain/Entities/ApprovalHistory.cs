using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

/// <summary>
/// Immutable record of a single approver decision within a leave request's workflow.
/// Each time a user approves, rejects, or sends back a request, one
/// <see cref="ApprovalHistory"/> entry is created.
/// </summary>
public class ApprovalHistory : BaseEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private ApprovalHistory() { }

    /// <summary>
    /// Creates a new approval history record.
    /// </summary>
    /// <param name="requestId">FK of the request this decision belongs to.</param>
    /// <param name="stageNumber">The workflow stage number at which the decision was made.</param>
    /// <param name="approverId">FK of the employee who made the decision.</param>
    /// <param name="decisionId">FK of the decision type (e.g. Approve, Reject, SendBack).</param>
    /// <param name="comment">Optional comment from the approver.</param>
    /// <param name="nextStageNumber">Stage to advance to after this decision, or <c>null</c> for terminal decisions.</param>
    /// <param name="decisionDate">UTC timestamp of the decision.</param>
    /// <param name="createdBy">ID of the user persisting this record.</param>
    public ApprovalHistory(int requestId, int stageNumber, int approverId,
        int decisionId, string? comment, int? nextStageNumber, DateTime decisionDate, int createdBy) : base(createdBy)
    {
        RequestId = requestId;
        StageNumber = stageNumber;
        ApproverId = approverId;
        DecisionId = decisionId;
        Comment = comment;
        NextStageNumber = nextStageNumber;
        DecisionDate = decisionDate;
    }

    /// <summary>Gets the FK of the associated leave request.</summary>
    public int RequestId { get; private set; }

    /// <summary>Gets the workflow stage number at which this decision occurred.</summary>
    public int StageNumber { get; private set; }

    /// <summary>Gets the FK of the employee who made the decision.</summary>
    public int ApproverId { get; private set; }

    /// <summary>Gets the FK of the decision type.</summary>
    public int DecisionId { get; private set; }

    /// <summary>Gets the UTC timestamp of the decision.</summary>
    public DateTime DecisionDate { get; private set; }

    /// <summary>Gets the approver's optional comment.</summary>
    public string? Comment { get; private set; }

    /// <summary>Gets the stage number to which the request was advanced, or <c>null</c> for terminal decisions.</summary>
    public int? NextStageNumber { get; private set; }

    /// <summary>Navigation property to the parent request.</summary>
    public virtual Request Request { get; private set; } = null!;

    /// <summary>Navigation property to the decision type reference.</summary>
    public virtual DecisionType Decision { get; private set; } = null!;
}
