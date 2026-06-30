using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

public class DecisionType : BaseEntity, IReferenceEntity
{
    private DecisionType() { }
    public DecisionType(string code, string name, bool isFinal, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        IsFinal = isFinal;
        SortOrder = sortOrder;
    }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsFinal { get; private set; }
    public int SortOrder { get; private set; }

}
