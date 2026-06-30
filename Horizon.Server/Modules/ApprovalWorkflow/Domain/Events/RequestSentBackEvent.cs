using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Events;

/// <summary>
/// Raised when a leave request is sent back to the employee for revision.
/// </summary>
public record RequestSentBackEvent(int RequestId, int EmployeeId, string RequestNumber, string? Comment) : IDomainEvent
{
    /// <inheritdoc />
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
