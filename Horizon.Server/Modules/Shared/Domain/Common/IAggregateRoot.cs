namespace Horizon.Server.Modules.Shared.Domain.Common;

/// <summary>
/// Marker interface that identifies an entity as a DDD aggregate root.
/// Only aggregate roots may be directly accessed via repositories.
/// </summary>
public interface IAggregateRoot { }
