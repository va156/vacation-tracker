using System.ComponentModel.DataAnnotations.Schema;

namespace Horizon.Server.Modules.Shared.Domain.Common;

public abstract class BaseEntity: ISoftDelete
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public int CreatedBy { get; protected set; }
    public int? UpdatedBy { get; protected set; }
    public bool IsActive { get; protected set; } = true;
    public DateTime? DeletedAt { get; protected set; }
    public int? DeletedBy { get; protected set; }


    private List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected BaseEntity() {}

    protected BaseEntity(int createdBy) : this()
    {
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow; 
        IsActive = true;
    }
    protected void UpdateAuditFields(int updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
    public void Delete(int deletedBy)
    {
        if (!IsActive) return;

        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        UpdateAuditFields(deletedBy);

        //AddDomainEvent(new EntityDeletedEvent(GetType().Name, Id, deletedBy));
    }

    public void Restore(int restoredBy)
    {
        if (IsActive) return;

        IsActive = true;
        DeletedAt = null;
        DeletedBy = null;
        UpdateAuditFields(restoredBy);
    }
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    protected void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
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
    public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
    public static bool operator ==(BaseEntity? left, BaseEntity? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity? left, BaseEntity? right) => !(left == right);

}
