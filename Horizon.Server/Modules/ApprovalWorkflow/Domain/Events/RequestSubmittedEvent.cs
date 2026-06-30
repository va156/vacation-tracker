using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Events;

/// <summary>
/// Raised when a leave request is submitted for approval.
/// Consumers can react by notifying approvers, updating balances, etc.
/// </summary>
public record RequestSubmittedEvent(int RequestId, int EmployeeId, string RequestNumber) : IDomainEvent
{
    /// <inheritdoc />
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
