using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.References.Domain.Entities;

/// <summary>
/// Reference entity that classifies the type of operation a leave request represents
/// (e.g. planning the year's leave, creating a new request, rescheduling, or cancelling).
/// Well-known codes are exposed as constants in the nested <see cref="Codes"/> class.
/// </summary>
public class OperationType : BaseEntity, IReferenceEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private OperationType() { }

    /// <summary>
    /// Creates a new operation type.
    /// </summary>
    /// <param name="code">Unique business code (see <see cref="Codes"/>).</param>
    /// <param name="name">Display name shown in the UI.</param>
    /// <param name="sortOrder">Display sort order.</param>
    /// <param name="createdBy">ID of the user creating this record.</param>
    public OperationType(string code, string name, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        SortOrder = sortOrder;
    }

    /// <summary>Gets the unique business code.</summary>
    public string Code { get; private set; } = null!;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = null!;

    /// <summary>Gets the display sort order.</summary>
    public int SortOrder { get; private set; }

    /// <summary>Well-known operation type codes. Use these instead of raw strings.</summary>
    public static class Codes
    {
        /// <summary>Annual leave schedule planning for the whole year.</summary>
        public const string PlanYear = "PLAN_YEAR";

        /// <summary>Requesting a new, unscheduled leave period.</summary>
        public const string NewLeave = "NEW_LEAVE";

        /// <summary>Moving an already-approved leave to different dates.</summary>
        public const string Reschedule = "RESCHEDULE";

        /// <summary>Cancelling a previously approved leave.</summary>
        public const string Cancel = "CANCEL";
    }
}
