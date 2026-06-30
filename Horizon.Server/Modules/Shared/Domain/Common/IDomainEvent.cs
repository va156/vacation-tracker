using MediatR;

namespace Horizon.Server.Modules.Shared.Domain.Common;

/// <summary>
/// Represents a domain event — a meaningful business occurrence that happened in the domain.
/// All domain events are also MediatR <see cref="INotification"/> instances so that they
/// can be dispatched through the MediatR pipeline.
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>Gets the UTC timestamp at which the event occurred.</summary>
    DateTime OccurredOn { get; }
}
