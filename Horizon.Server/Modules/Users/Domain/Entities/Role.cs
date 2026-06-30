using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Users.Domain.Entities;

public class Role : BaseEntity, IReferenceEntity
{
    private Role() { }
    public Role(string code, string name, string? description, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        Description = description; 
        SortOrder = sortOrder;
    }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int SortOrder { get; private set; }


    private readonly List<User> _users = new();
    public virtual IReadOnlyCollection<User> Users => _users.AsReadOnly();

    private readonly List<ApprovalStage> _approvalStages = new();
    public virtual IReadOnlyCollection<ApprovalStage> ApprovalStages => _approvalStages.AsReadOnly();
}