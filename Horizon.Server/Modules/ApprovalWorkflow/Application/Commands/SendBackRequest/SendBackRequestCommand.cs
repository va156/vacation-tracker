using MediatR;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.SendBackRequest;

/// <summary>Command sent when an approver returns a request to the employee for revision.</summary>
public class SendBackRequestCommand : IRequest
{
    /// <summary>Gets or sets the ID of the request being sent back.</summary>
    public int RequestId { get; set; }

    /// <summary>Gets or sets the workflow stage at which the decision is made.</summary>
    public int StageNumber { get; set; }

    /// <summary>Gets or sets the ID of the employee making the decision.</summary>
    public int ApproverId { get; set; }

    /// <summary>Gets or sets the comment explaining what needs to be revised.</summary>
    public string? Comment { get; set; }
}
