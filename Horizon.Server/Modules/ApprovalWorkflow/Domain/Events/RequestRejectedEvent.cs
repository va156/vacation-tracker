using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Events;

/// <summary>
/// Raised when a leave request is rejected by an approver.
/// </summary>
public record RequestRejectedEvent(int RequestId, int EmployeeId, string RequestNumber, string? Reason) : IDomainEvent
{
    /// <inheritdoc />
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
