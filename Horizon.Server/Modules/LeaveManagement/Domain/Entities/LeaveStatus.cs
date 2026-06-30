using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.LeaveManagement.Domain.Entities;

public class LeaveStatus : BaseEntity, IReferenceEntity
{
    private LeaveStatus() {}
    public LeaveStatus(string code, string name, int sortOrder, int createdBy) : base(createdBy)
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
        public const string Planned = "PLANNED";
        public const string Approved = "APPROVED";
        public const string Used = "USED";
        public const string Cancelled = "CANCELLED";
        public const string Rescheduled = "RESCHEDULED";
    }

}