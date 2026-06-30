using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

public class RequestStatus : BaseEntity, IReferenceEntity
{
    private RequestStatus() { }

    public RequestStatus(string code, string name, int sortOrder, int createdBy) : base(createdBy)
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
        public const string Draft = "DRAFT";
        public const string PendingManager = "PENDING_MANAGER";
        public const string PendingHr = "PENDING_HR";
        public const string Approved = "APPROVED";
        public const string Rejected = "REJECTED";
        public const string SentBack = "SENT_BACK";
    }

}
