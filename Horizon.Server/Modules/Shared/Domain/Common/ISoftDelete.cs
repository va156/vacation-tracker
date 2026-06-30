namespace Horizon.Server.Modules.Shared.Domain.Common;

/// <summary>
/// Contract for entities that support soft deletion.
/// Rather than being physically removed from the database, a soft-deleted entity has
/// <see cref="IsActive"/> set to <c>false</c> and retains its audit timestamps.
/// </summary>
public interface ISoftDelete
{
    /// <summary>Gets <c>true</c> when the entity has not been soft-deleted.</summary>
    bool IsActive { get; }

    /// <summary>Gets the UTC timestamp when the entity was soft-deleted, or <c>null</c> if still active.</summary>
    DateTime? DeletedAt { get; }

    /// <summary>Gets the ID of the user who soft-deleted the entity, or <c>null</c> if still active.</summary>
    int? DeletedBy { get; }
}
