using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Employees.Domain.Entities;

/// <summary>
/// Aggregate root representing an organisational department.
/// Departments form a tree hierarchy via the <see cref="Parent"/> / <see cref="Children"/>
/// self-reference. Each department may have an <see cref="Employee"/> designated as its manager.
/// </summary>
public class Department : BaseEntity, IAggregateRoot
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private Department() { }

    /// <summary>
    /// Creates a new department.
    /// </summary>
    /// <param name="code">Unique business code (e.g. "IT", "HR").</param>
    /// <param name="name">Display name.</param>
    /// <param name="parentId">FK of the parent department, or <c>null</c> for a root department.</param>
    /// <param name="managerId">FK of the managing employee, or <c>null</c>.</param>
    /// <param name="createdBy">ID of the user creating the department.</param>
    public Department(string code, string name, int? parentId, int? managerId, int createdBy)
        : base(createdBy)
    {
        Code = code;
        Name = name;
        ParentId = parentId;
        ManagerId = managerId;
    }

    /// <summary>Gets the FK of the parent department, or <c>null</c> for a root node.</summary>
    public int? ParentId { get; private set; }

    /// <summary>Gets the unique business code for this department.</summary>
    public string Code { get; private set; } = null!;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = null!;

    /// <summary>Gets the FK of the employee managing this department, or <c>null</c>.</summary>
    public int? ManagerId { get; private set; }

    /// <summary>Navigation property to the parent department.</summary>
    public virtual Department? Parent { get; private set; }

    /// <summary>Navigation property to the managing employee.</summary>
    public virtual Employee? Manager { get; private set; }

    private readonly List<Department> _children = new();
    /// <summary>Gets the direct child departments.</summary>
    public virtual IReadOnlyCollection<Department> Children => _children.AsReadOnly();

    private readonly List<EmployeeDepartment> _employeeAssignments = new();
    /// <summary>Gets all employee-department assignment records for this department.</summary>
    public virtual IReadOnlyCollection<EmployeeDepartment> EmployeeAssignments => _employeeAssignments.AsReadOnly();

    private readonly List<ApprovalStage> _approvalStages = new();
    /// <summary>Gets approval template stages that are scoped to this department.</summary>
    public virtual IReadOnlyCollection<ApprovalStage> ApprovalStages => _approvalStages.AsReadOnly();
}
