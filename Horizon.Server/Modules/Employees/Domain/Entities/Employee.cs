using Horizon.Server.Modules.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.Employees.Domain.Entities;

/// <summary>
/// Aggregate root that represents a company employee.
/// An employee is a HR record that exists independently from a user account.
/// When a user account is linked (<see cref="HasUserAccount"/> = <c>true</c>),
/// the employee can log in and submit leave requests.
/// The hierarchy is modelled via the <see cref="Manager"/> self-reference.
/// </summary>
[Index(nameof(EmployeeNumber), IsUnique = true)]
public class Employee : BaseEntity, IAggregateRoot
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private Employee() { }

    /// <summary>
    /// Creates a new employee record.
    /// </summary>
    /// <param name="employeeNumber">Unique HR identifier (e.g. "EMP-0001").</param>
    /// <param name="hireDate">Date the employee joined the company.</param>
    /// <param name="createdBy">ID of the HR user creating the record.</param>
    public Employee(string employeeNumber, DateTime hireDate, int createdBy)
        : base(createdBy)
    {
        EmployeeNumber = employeeNumber;
        HireDate = hireDate;
        HasUserAccount = false;
    }

    /// <summary>Gets the unique HR employee number.</summary>
    public string EmployeeNumber { get; private set; } = null!;

    /// <summary>Gets the date the employee joined the company.</summary>
    public DateTime HireDate { get; private set; }

    /// <summary>Gets the date the employee left the company, or <c>null</c> if still employed.</summary>
    public DateTime? TerminationDate { get; private set; }

    /// <summary>Gets the FK of the employee's direct manager, or <c>null</c> for top-level employees.</summary>
    public int? ManagerId { get; private set; }

    /// <summary>Gets <c>true</c> when this employee record is linked to a system user account.</summary>
    public bool HasUserAccount { get; private set; }

    /// <summary>Navigation property to the direct manager.</summary>
    public virtual Employee? Manager { get; private set; }

    private readonly List<Employee> _subordinates = new();
    /// <summary>Gets all employees who report directly to this employee.</summary>
    public virtual IReadOnlyCollection<Employee> Subordinates => _subordinates.AsReadOnly();

    private readonly List<EmployeeDepartment> _departmentAssignments = new();
    /// <summary>Gets all department assignments (current and historical) for this employee.</summary>
    public virtual IReadOnlyCollection<EmployeeDepartment> DepartmentAssignments => _departmentAssignments.AsReadOnly();

    private readonly List<Department> _managedDepartments = new();
    /// <summary>Gets all departments where this employee serves as manager.</summary>
    public virtual IReadOnlyCollection<Department> ManagedDepartments => _managedDepartments.AsReadOnly();
}
