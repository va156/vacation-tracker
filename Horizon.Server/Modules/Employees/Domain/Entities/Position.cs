using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Employees.Domain.Entities;

public class Position : BaseEntity
{
    private Position() { }

    public Position(string code, string name, string? description, int? grade, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        Description = description;  
        Grade = grade;
    }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int? Grade { get; private set; }
    public string? Description { get; private set; }

    private readonly List<Employee> _employees = new();
    public virtual IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();


    private readonly List<EmployeeDepartment> _employeeDepartments = new();
    public virtual IReadOnlyCollection<EmployeeDepartment> EmployeeDepartments => _employeeDepartments.AsReadOnly();


    private readonly List<ApprovalStage> _approvalStages = new();
    public virtual IReadOnlyCollection<ApprovalStage> ApprovalStages => _approvalStages.AsReadOnly();
}