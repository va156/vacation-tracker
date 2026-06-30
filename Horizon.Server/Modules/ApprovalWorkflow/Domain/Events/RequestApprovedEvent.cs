using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Events;

/// <summary>
/// Raised when a leave request is fully approved at all workflow stages.
/// Consumers can react by updating leave balances and notifying the employee.
/// </summary>
public record RequestApprovedEvent(int RequestId, int EmployeeId, string RequestNumber) : IDomainEvent
{
    /// <inheritdoc />
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
