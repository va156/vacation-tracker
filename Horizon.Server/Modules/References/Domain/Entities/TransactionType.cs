using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.References.Domain.Entities;

public class TransactionType : BaseEntity, IReferenceEntity
{
    private TransactionType() { }
    public TransactionType(string code, string name, string sign, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        Sign = sign;
        SortOrder = sortOrder;
    }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Sign { get; private set; } = null!; // "+" или "-"
    public int SortOrder { get; private set; }

}
