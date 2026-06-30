using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

/// <summary>
/// Defines a reusable approval workflow template.
/// A template contains an ordered list of <see cref="ApprovalStage"/> records that
/// describe who must approve a request and in which order. Templates are assigned to
/// requests at submission time.
/// </summary>
public class ApprovalTemplate : BaseEntity, IReferenceEntity, IAggregateRoot
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private ApprovalTemplate() { }

    /// <summary>
    /// Creates a new approval template.
    /// </summary>
    /// <param name="code">Unique business code (e.g. "STANDARD_LEAVE").</param>
    /// <param name="name">Display name shown in the UI.</param>
    /// <param name="sortOrder">Order when listing templates.</param>
    /// <param name="createdBy">ID of the user creating the template.</param>
    public ApprovalTemplate(string code, string name, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        SortOrder = sortOrder;
    }

    /// <summary>Gets the unique business code for this template.</summary>
    public string Code { get; private set; } = null!;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = null!;

    /// <summary>Gets the display sort order.</summary>
    public int SortOrder { get; private set; }

    /// <summary>Gets the optional description of when this template should be used.</summary>
    public string? Description { get; private set; }

    private readonly List<ApprovalStage> _stages = new();
    /// <summary>Gets the ordered list of approval stages that make up this workflow.</summary>
    public virtual IReadOnlyCollection<ApprovalStage> Stages => _stages.AsReadOnly();
}
