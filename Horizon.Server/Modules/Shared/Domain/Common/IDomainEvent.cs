using MediatR;

namespace Horizon.Server.Modules.Shared.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}