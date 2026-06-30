using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

/// <summary>
/// Reference entity that classifies an approver's decision (e.g. Approve, Reject, SendBack).
/// <see cref="IsFinal"/> controls whether a decision terminates the workflow (no further stages).
/// </summary>
public class DecisionType : BaseEntity, IReferenceEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private DecisionType() { }

    /// <summary>
    /// Creates a new decision type.
    /// </summary>
    /// <param name="code">Unique business code (e.g. "APPROVE", "REJECT").</param>
    /// <param name="name">Display name shown in the UI.</param>
    /// <param name="isFinal"><c>true</c> if this decision terminates the workflow immediately.</param>
    /// <param name="sortOrder">Display order in decision lists.</param>
    /// <param name="createdBy">ID of the user creating this record.</param>
    public DecisionType(string code, string name, bool isFinal, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        IsFinal = isFinal;
        SortOrder = sortOrder;
    }

    /// <summary>Gets the unique business code.</summary>
    public string Code { get; private set; } = null!;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Gets <c>true</c> when this decision ends the workflow (e.g. Reject or final Approve),
    /// and <c>false</c> for intermediate decisions that advance to the next stage.
    /// </summary>
    public bool IsFinal { get; private set; }

    /// <summary>Gets the display sort order.</summary>
    public int SortOrder { get; private set; }
}
