using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

public class ApprovalHistory : BaseEntity
{
    private ApprovalHistory() { }

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

    public int RequestId { get; private set; }
    public int StageNumber { get; private set; }
    public int ApproverId { get; private set; }
    public int DecisionId { get; private set; }
    public DateTime DecisionDate { get; private set; }
    public string? Comment { get; private set; }
    public int? NextStageNumber { get; private set; }

    // Navigation 
    public virtual Request Request { get; private set; } = null!;
    public virtual DecisionType Decision { get; private set; } = null!;
}