using Horizon.Server.Modules.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.Employees.Domain.Entities;

[Index(nameof(EmployeeNumber), IsUnique = true)]
public class Employee : BaseEntity, IAggregateRoot
{
    private Employee() { }

    public Employee(string employeeNumber, DateTime hireDate, int createdBy)
        : base(createdBy)
    {
        EmployeeNumber = employeeNumber;
        HireDate = hireDate;
        HasUserAccount = false;
    }

    public string EmployeeNumber { get; private set; } = null!;
    public DateTime HireDate { get; private set; }
    public DateTime? TerminationDate { get; private set; }
    public int? ManagerId { get; private set; }
    public bool HasUserAccount { get; private set; } 
    public virtual Employee? Manager { get; private set; }


    private readonly List<Employee> _subordinates = new();
    public virtual IReadOnlyCollection<Employee> Subordinates => _subordinates.AsReadOnly();


    private readonly List<EmployeeDepartment> _departmentAssignments = new();
    public virtual IReadOnlyCollection<EmployeeDepartment> DepartmentAssignments => _departmentAssignments.AsReadOnly();


    private readonly List<Department> _managedDepartments = new();
    public virtual IReadOnlyCollection<Department> ManagedDepartments => _managedDepartments.AsReadOnly();

}