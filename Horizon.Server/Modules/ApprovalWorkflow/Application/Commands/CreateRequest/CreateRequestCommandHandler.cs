using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;
using Horizon.Server.Modules.ApprovalWorkflow.Domain.Interfaces;
using Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Services;
using Horizon.Server.Modules.LeaveManagement.Domain.Entities;
using Horizon.Server.Modules.Shared.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.CreateRequest;

/// <summary>
/// Handles <see cref="CreateRequestCommand"/> by creating a new leave <see cref="Request"/>
/// together with its associated <see cref="Leave"/> records and persisting them in a single
/// atomic transaction.
/// </summary>
/// <remarks>
/// Workflow:
/// <list type="number">
///   <item>Generate a unique, thread-safe request number via <see cref="RequestNumberGenerator"/>.</item>
///   <item>Resolve the initial status (<c>PENDING_MANAGER</c>) from the database by its code.</item>
///   <item>Create the <see cref="Request"/> aggregate and attach all leave periods.</item>
///   <item>Persist everything in one transaction via <see cref="IUnitOfWork"/>.</item>
/// </list>
/// </remarks>
public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, int>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RequestNumberGenerator _numberGenerator;
    private readonly AppDbContext _db;

    /// <summary>Initialises the handler with its required dependencies.</summary>
    public CreateRequestCommandHandler(
        IRequestRepository requestRepository,
        IUnitOfWork unitOfWork,
        RequestNumberGenerator numberGenerator,
        AppDbContext db)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _numberGenerator = numberGenerator;
        _db = db;
    }

    /// <inheritdoc />
    public async Task<int> Handle(CreateRequestCommand command, CancellationToken cancellationToken)
    {
        // 1. Generate a unique request number (protected by SemaphoreSlim inside the generator).
        var requestNumber = await _numberGenerator.GenerateAsync(DateTime.UtcNow.Year, cancellationToken);

        // 2. Resolve PENDING_MANAGER status ID — fail fast if reference data is missing.
        var pendingManagerStatus = await _db.Set<RequestStatus>()
            .FirstOrDefaultAsync(s => s.Code == RequestStatus.Codes.PendingManager, cancellationToken)
            ?? throw new InvalidOperationException(
                $"Required reference data missing: RequestStatus with code '{RequestStatus.Codes.PendingManager}' not found.");

        // 3. Resolve PLANNED leave status ID.
        var plannedLeaveStatus = await _db.Set<LeaveStatus>()
            .FirstOrDefaultAsync(s => s.Code == LeaveStatus.Codes.Planned, cancellationToken)
            ?? throw new InvalidOperationException(
                $"Required reference data missing: LeaveStatus with code '{LeaveStatus.Codes.Planned}' not found.");

        int requestId = 0;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            // 4. Create the Request aggregate.
            var request = new Request(
                requestNumber,
                command.OperationTypeId,
                command.EmployeeId,
                command.DepartmentId,
                command.ApprovalTemplateId,
                command.EmployeeId);

            // 5. Set initial workflow status via EF shadow property (StatusId).
            _db.Entry(request).Property("StatusId").CurrentValue = pendingManagerStatus.Id;

            // 6. Persist the request first to obtain its database-generated Id.
            await _requestRepository.AddAsync(request);
            await _unitOfWork.SaveChangesAsync();

            // 7. Attach leave periods now that we have the request Id.
            foreach (var leaveDto in command.Leaves)
            {
                var duration = (decimal)(leaveDto.EndDate.Date - leaveDto.StartDate.Date).TotalDays + 1;
                var leave = new Leave(
                    request.Id,
                    command.EmployeeId,
                    leaveDto.LeaveTypeId,
                    leaveDto.StartDate,
                    leaveDto.EndDate,
                    duration,
                    command.EmployeeId);

                // Set initial leave status.
                _db.Entry(leave).Property("StatusId").CurrentValue = plannedLeaveStatus.Id;
                await _db.Set<Leave>().AddAsync(leave, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync();
            requestId = request.Id;
        });

        return requestId;
    }
}
