using Horizon.Server.Modules.Employees.Domain.Entities;
using Horizon.Server.Modules.LeaveManagement.Domain.Entities;
using Horizon.Server.Modules.References.Domain.Entities;
using Horizon.Server.Modules.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

[Index(nameof(RequestNumber), IsUnique = true)]
[Index(nameof(EmployeeId))]
[Index(nameof(StatusId))]
public class Request : BaseEntity, IAggregateRoot
{
    private Request() { }
    public Request(string requestNumber, int operationTypeId, int employeeId,
            int departmentId, int approvalTemplateId, int createdBy) : base(createdBy)
    {
        RequestNumber = requestNumber;
        OperationTypeId = operationTypeId;
        EmployeeId = employeeId;
        DepartmentId = departmentId;
        ApprovalTemplateId = approvalTemplateId;
        _leaves = new List<Leave>();
    }

    public string RequestNumber { get; private set; } = null!;
    public int OperationTypeId { get; private set; }
    public int StatusId { get; private set; }
    public int EmployeeId { get; private set; }
    public int DepartmentId { get; private set; }
    public int ApprovalTemplateId { get; private set; }
    public string? Comment { get; private set; }
    public int? CurrentStageNumber { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public virtual RequestStatus Status { get; private set; } = null!;
    public virtual ApprovalTemplate ApprovalTemplate { get; private set; } = null!;


    private readonly List<Leave> _leaves;
    public virtual IReadOnlyCollection<Leave> Leaves => _leaves.AsReadOnly();


    private readonly List<ApprovalHistory> _approvalHistory = new();
    public virtual IReadOnlyCollection<ApprovalHistory> ApprovalHistory => _approvalHistory.AsReadOnly();
}