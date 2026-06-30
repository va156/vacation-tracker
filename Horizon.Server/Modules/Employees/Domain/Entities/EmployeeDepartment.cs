using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Employees.Domain.Entities;

public class EmployeeDepartment : BaseEntity
{
    private EmployeeDepartment() { }
    public EmployeeDepartment(int employeeId, int departmentId, int positionId,
        DateTime startDate, bool isPrimary, decimal fte, int createdBy) : base(createdBy)
    {
        EmployeeId = employeeId;
        DepartmentId = departmentId;
        PositionId = positionId;
        StartDate = startDate;
        IsPrimary = isPrimary;
        FTE = fte;
    }

    public int EmployeeId { get; private set; }
    public int DepartmentId { get; private set; }
    public int PositionId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public bool IsPrimary { get; private set; }
    public decimal FTE { get; private set; }
    public virtual Employee Employee { get; private set; } = null!;
    public virtual Department Department { get; private set; } = null!;
    public virtual Position Position { get; private set; } = null!;
}