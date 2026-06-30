using Horizon.Server.Modules.ApprovalWorkflow.Domain.Events;
using MediatR;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.EventHandlers;

/// <summary>
/// Handles <see cref="RequestSubmittedEvent"/> raised when an employee submits a new leave request.
/// Currently logs the submission for audit purposes. Can be extended to send notifications,
/// reserve balances, or trigger external integrations without modifying the submission flow.
/// </summary>
/// <remarks>
/// This class demonstrates the <b>Observer / Domain Events</b> design pattern:
/// the <see cref="Request"/> aggregate raises an event, and this handler reacts
/// independently — neither side knows about the other directly.
/// </remarks>
public class HandleLeaveRequestedEvent : INotificationHandler<RequestSubmittedEvent>
{
    private readonly ILogger<HandleLeaveRequestedEvent> _logger;

    /// <summary>Initialises the handler with a logger.</summary>
    public HandleLeaveRequestedEvent(ILogger<HandleLeaveRequestedEvent> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task Handle(RequestSubmittedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Leave request submitted: RequestId={RequestId}, RequestNumber={RequestNumber}, EmployeeId={EmployeeId}, OccurredOn={OccurredOn}",
            notification.RequestId,
            notification.RequestNumber,
            notification.EmployeeId,
            notification.OccurredOn);

        // TODO: extend with notifications, balance reservation, etc.
        return Task.CompletedTask;
    }
}
