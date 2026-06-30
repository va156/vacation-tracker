using MediatR;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.RejectRequest;

/// <summary>Command sent when an approver rejects a leave request.</summary>
public class RejectRequestCommand : IRequest
{
    /// <summary>Gets or sets the ID of the request being rejected.</summary>
    public int RequestId { get; set; }

    /// <summary>Gets or sets the workflow stage at which the decision is made.</summary>
    public int StageNumber { get; set; }

    /// <summary>Gets or sets the ID of the employee rejecting the request.</summary>
    public int ApproverId { get; set; }

    /// <summary>Gets or sets the reason for rejection (optional but recommended).</summary>
    public string? Comment { get; set; }
}
