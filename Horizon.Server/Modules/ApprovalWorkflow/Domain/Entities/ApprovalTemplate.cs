using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

public class ApprovalTemplate : BaseEntity, IReferenceEntity, IAggregateRoot
{
    private ApprovalTemplate() {}
    public ApprovalTemplate(string code, string name, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        SortOrder = sortOrder;
    }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int SortOrder { get; private set; }
    public string? Description { get; private set; }

    // Navigation
    private readonly List<ApprovalStage> _stages = new();
    public virtual IReadOnlyCollection<ApprovalStage> Stages => _stages.AsReadOnly();
}
