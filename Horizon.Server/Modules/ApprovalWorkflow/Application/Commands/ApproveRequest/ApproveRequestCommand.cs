using MediatR;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.ApproveRequest;

/// <summary>Command sent when an approver accepts a leave request at a given stage.</summary>
public class ApproveRequestCommand : IRequest
{
    /// <summary>Gets or sets the ID of the request being approved.</summary>
    public int RequestId { get; set; }

    /// <summary>Gets or sets the workflow stage at which the decision is made.</summary>
    public int StageNumber { get; set; }

    /// <summary>Gets or sets the ID of the employee approving the request.</summary>
    public int ApproverId { get; set; }

    /// <summary>Gets or sets an optional comment from the approver.</summary>
    public string? Comment { get; set; }
}
