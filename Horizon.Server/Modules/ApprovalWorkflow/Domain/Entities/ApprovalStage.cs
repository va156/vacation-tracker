using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;
using Horizon.Server.Modules.Shared.Domain.Common;

/// <summary>
/// Represents a single stage within an <see cref="ApprovalTemplate"/>.
/// Each stage specifies who is required to act (by role, department, or position)
/// and optional routing constraints such as a timeout.
/// Stages are executed in ascending <see cref="StageNumber"/> order.
/// </summary>
public class ApprovalStage : BaseEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private ApprovalStage() { }

    /// <summary>
    /// Creates a new stage within an approval template.
    /// </summary>
    /// <param name="templateId">FK of the owning <see cref="ApprovalTemplate"/>.</param>
    /// <param name="stageNumber">Sequential position of this stage (1-based).</param>
    /// <param name="roleId">FK of the role required to approve at this stage, or <c>null</c>.</param>
    /// <param name="departmentId">FK of the department required to approve, or <c>null</c>.</param>
    /// <param name="positionId">FK of the position required to approve, or <c>null</c>.</param>
    /// <param name="stageName">Human-readable name for this stage.</param>
    /// <param name="timeoutHours">Hours after submission before the stage auto-escalates, or <c>null</c>.</param>
    /// <param name="isRequired"><c>true</c> if the stage cannot be skipped.</param>
    /// <param name="createdBy">ID of the user configuring the stage.</param>
    public ApprovalStage(int templateId, int stageNumber, int? roleId, int? departmentId,
        int? positionId, string? stageName, int? timeoutHours, bool isRequired, int createdBy)
        : base(createdBy)
    {
        TemplateId = templateId;
        StageNumber = stageNumber;
        RoleId = roleId;
        DepartmentId = departmentId;
        PositionId = positionId;
        StageName = stageName;
        TimeoutHours = timeoutHours;
        IsRequired = isRequired;
    }

    /// <summary>Gets the FK of the parent approval template.</summary>
    public int TemplateId { get; private set; }

    /// <summary>Gets the sequential stage number (1-based).</summary>
    public int StageNumber { get; private set; }

    /// <summary>Gets the FK of the role that must approve at this stage, or <c>null</c>.</summary>
    public int? RoleId { get; private set; }

    /// <summary>Gets the FK restricting approval to a specific department, or <c>null</c>.</summary>
    public int? DepartmentId { get; private set; }

    /// <summary>Gets the FK restricting approval to a specific position, or <c>null</c>.</summary>
    public int? PositionId { get; private set; }

    /// <summary>Gets the display name for this approval stage.</summary>
    public string? StageName { get; private set; }

    /// <summary>Gets the number of hours before escalation, or <c>null</c> if no timeout applies.</summary>
    public int? TimeoutHours { get; private set; }

    /// <summary>Gets <c>true</c> when this stage cannot be bypassed in the workflow.</summary>
    public bool IsRequired { get; private set; }

    /// <summary>Navigation property to the parent template.</summary>
    public virtual ApprovalTemplate Template { get; private set; } = null!;
}
