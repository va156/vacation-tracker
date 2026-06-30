using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Employees.Domain.Entities;

public class Department : BaseEntity, IAggregateRoot
{
    private Department() { }
    public Department(string code, string name, int? parentId, int? managerId, int createdBy)
        : base(createdBy)
    {
        Code = code;
        Name = name;
        ParentId = parentId;
        ManagerId = managerId;
    }

    public int? ParentId { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int? ManagerId { get; private set; }
    public virtual Department? Parent { get; private set; }
    public virtual Employee? Manager { get; private set; }


    private readonly List<Department> _children = new();
    public virtual IReadOnlyCollection<Department> Children => _children.AsReadOnly();


    private readonly List<EmployeeDepartment> _employeeAssignments = new();
    public virtual IReadOnlyCollection<EmployeeDepartment> EmployeeAssignments => _employeeAssignments.AsReadOnly();


    private readonly List<ApprovalStage> _approvalStages = new();
    public virtual IReadOnlyCollection<ApprovalStage> ApprovalStages => _approvalStages.AsReadOnly();
}