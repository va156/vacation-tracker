using MediatR;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.CreateRequest;

public class CreateRequestCommand : IRequest<int>
{
    public int OperationTypeId { get; set; }
    public int EmployeeId { get; set; }
    public int DepartmentId { get; set; }
    public int ApprovalTemplateId { get; set; }
    public string? Comment { get; set; }
    public List<LeaveDto> Leaves { get; set; } = new();
}

public class LeaveDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int LeaveTypeId { get; set; }
}
