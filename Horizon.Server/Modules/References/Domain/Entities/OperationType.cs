using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.References.Domain.Entities;

public class OperationType : BaseEntity, IReferenceEntity
{
    private OperationType() { }
    public OperationType(string code, string name, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        SortOrder = sortOrder;
    }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int SortOrder { get; private set; }

    public static class Codes
    {
        public const string PlanYear = "PLAN_YEAR";
        public const string NewLeave = "NEW_LEAVE";
        public const string Reschedule = "RESCHEDULE";
        public const string Cancel = "CANCEL";
    }
}