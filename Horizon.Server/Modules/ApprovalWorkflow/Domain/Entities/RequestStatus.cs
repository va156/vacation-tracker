using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

/// <summary>
/// Reference entity representing the lifecycle status of a leave request.
/// Well-known status codes are exposed as constants in the nested <see cref="Codes"/> class
/// to avoid magic strings in application code.
/// </summary>
public class RequestStatus : BaseEntity, IReferenceEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private RequestStatus() { }

    /// <summary>
    /// Creates a new request status entry.
    /// </summary>
    /// <param name="code">Unique business code (see <see cref="Codes"/>).</param>
    /// <param name="name">Display name shown in the UI.</param>
    /// <param name="sortOrder">Display order in status lists.</param>
    /// <param name="createdBy">ID of the user creating this status.</param>
    public RequestStatus(string code, string name, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        SortOrder = sortOrder;
    }

    /// <summary>Gets the unique business code for this status.</summary>
    public string Code { get; private set; } = null!;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = null!;

    /// <summary>Gets the display sort order.</summary>
    public int SortOrder { get; private set; }

    /// <summary>
    /// Well-known status code constants. Use these instead of raw strings to prevent typos
    /// and enable reliable searching.
    /// </summary>
    public static class Codes
    {
        /// <summary>The request has been created but not yet submitted.</summary>
        public const string Draft = "DRAFT";

        /// <summary>Awaiting approval from the department manager.</summary>
        public const string PendingManager = "PENDING_MANAGER";

        /// <summary>Awaiting approval from HR.</summary>
        public const string PendingHr = "PENDING_HR";

        /// <summary>The request has been fully approved.</summary>
        public const string Approved = "APPROVED";

        /// <summary>The request has been rejected at any stage.</summary>
        public const string Rejected = "REJECTED";

        /// <summary>The request was returned to the employee for revision.</summary>
        public const string SentBack = "SENT_BACK";
    }
}
