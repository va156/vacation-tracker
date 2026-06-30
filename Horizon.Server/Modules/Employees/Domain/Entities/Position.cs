using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Employees.Domain.Entities;

/// <summary>
/// Represents a job position (e.g. "Senior Developer", "HR Specialist").
/// Positions are assigned to employees through <see cref="EmployeeDepartment"/> join records,
/// allowing an employee to hold different positions in different departments.
/// <see cref="Grade"/> is an optional numeric seniority level.
/// </summary>
public class Position : BaseEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private Position() { }

    /// <summary>
    /// Creates a new job position.
    /// </summary>
    /// <param name="code">Unique business code (e.g. "SWE-SR").</param>
    /// <param name="name">Display name (e.g. "Senior Software Engineer").</param>
    /// <param name="description">Optional description of the position's responsibilities.</param>
    /// <param name="grade">Optional numeric seniority grade, or <c>null</c>.</param>
    /// <param name="createdBy">ID of the user creating the position.</param>
    public Position(string code, string name, string? description, int? grade, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        Description = description;
        Grade = grade;
    }

    /// <summary>Gets the unique business code for this position.</summary>
    public string Code { get; private set; } = null!;

    /// <summary>Gets the display name of the position.</summary>
    public string Name { get; private set; } = null!;

    /// <summary>Gets the optional numeric seniority grade.</summary>
    public int? Grade { get; private set; }

    /// <summary>Gets the optional description of the position's scope and responsibilities.</summary>
    public string? Description { get; private set; }

    private readonly List<Employee> _employees = new();
    /// <summary>Gets employees currently holding this position (across all departments).</summary>
    public virtual IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    private readonly List<EmployeeDepartment> _employeeDepartments = new();
    /// <summary>Gets all department assignment records that reference this position.</summary>
    public virtual IReadOnlyCollection<EmployeeDepartment> EmployeeDepartments => _employeeDepartments.AsReadOnly();

    private readonly List<ApprovalStage> _approvalStages = new();
    /// <summary>Gets approval template stages that require approval from this position.</summary>
    public virtual IReadOnlyCollection<ApprovalStage> ApprovalStages => _approvalStages.AsReadOnly();
}
