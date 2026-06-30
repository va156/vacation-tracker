using System.ComponentModel.DataAnnotations.Schema;

namespace Horizon.Server.Modules.Shared.Domain.Common;

/// <summary>
/// Base class for all domain entities. Provides identity, audit trail, soft-delete
/// semantics, and domain-event support.
/// </summary>
public abstract class BaseEntity : ISoftDelete
{
    /// <summary>Gets the entity's surrogate primary key.</summary>
    public int Id { get; protected set; }

    /// <summary>Gets the UTC timestamp when the entity was created.</summary>
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>Gets the UTC timestamp of the last update, or <c>null</c> if never updated.</summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>Gets the ID of the user who created the entity.</summary>
    public int CreatedBy { get; protected set; }

    /// <summary>Gets the ID of the user who last updated the entity, or <c>null</c>.</summary>
    public int? UpdatedBy { get; protected set; }

    /// <summary>
    /// Gets whether the entity is logically active (not soft-deleted).
    /// Always <c>true</c> upon creation; set to <c>false</c> by <see cref="Delete"/>.
    /// </summary>
    public bool IsActive { get; protected set; } = true;

    /// <summary>Gets the UTC timestamp when the entity was soft-deleted, or <c>null</c>.</summary>
    public DateTime? DeletedAt { get; protected set; }

    /// <summary>Gets the ID of the user who performed the soft-delete, or <c>null</c>.</summary>
    public int? DeletedBy { get; protected set; }

    private List<IDomainEvent> _domainEvents = new();

    /// <summary>Gets the domain events raised by this entity since the last dispatch.</summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>Parameterless constructor for EF Core materialisation.</summary>
    protected BaseEntity() { }

    /// <summary>
    /// Initialises a new entity and records who created it.
    /// </summary>
    /// <param name="createdBy">ID of the creating user (0 = system).</param>
    protected BaseEntity(int createdBy) : this()
    {
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Updates the audit fields to reflect a modification.
    /// </summary>
    /// <param name="updatedBy">ID of the user performing the update.</param>
    protected void UpdateAuditFields(int updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    /// <summary>
    /// Soft-deletes the entity. Has no effect if the entity is already inactive.
    /// </summary>
    /// <param name="deletedBy">ID of the user requesting deletion.</param>
    public void Delete(int deletedBy)
    {
        if (!IsActive) return;

        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        UpdateAuditFields(deletedBy);
    }

    /// <summary>
    /// Restores a previously soft-deleted entity. Has no effect if the entity is already active.
    /// </summary>
    /// <param name="restoredBy">ID of the user performing the restore.</param>
    public void Restore(int restoredBy)
    {
        if (IsActive) return;

        IsActive = true;
        DeletedAt = null;
        DeletedBy = null;
        UpdateAuditFields(restoredBy);
    }

    /// <summary>Enqueues a domain event to be dispatched after the current operation.</summary>
    /// <param name="domainEvent">The domain event to add.</param>
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>Removes a previously enqueued domain event.</summary>
    /// <param name="domainEvent">The domain event to remove.</param>
    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    /// <summary>Removes all enqueued domain events. Typically called after events have been dispatched.</summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    /// <inheritdoc />
    public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();

    /// <summary>Compares two entities by type and ID.</summary>
    public static bool operator ==(BaseEntity? left, BaseEntity? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    /// <summary>Returns <c>true</c> when the two entities are not equal.</summary>
    public static bool operator !=(BaseEntity? left, BaseEntity? right) => !(left == right);
}
