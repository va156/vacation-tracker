using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.References.Domain.Entities;

public class LeaveType : BaseEntity, IReferenceEntity
{
    private LeaveType() { }
    public LeaveType(string code, string name, bool isPaid, bool affectsBalance,
        int minDays, int maxDays, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        IsPaid = isPaid;
        AffectsBalance = affectsBalance;
        MinDays = minDays;
        MaxDays = maxDays;
        SortOrder = sortOrder;
    }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsPaid { get; private set; }
    public bool AffectsBalance { get; private set; }
    public int MinDays { get; private set; }
    public int MaxDays { get; private set; }
    public decimal? AccrualRate { get; private set; }
    public int SortOrder { get; private set; }

}