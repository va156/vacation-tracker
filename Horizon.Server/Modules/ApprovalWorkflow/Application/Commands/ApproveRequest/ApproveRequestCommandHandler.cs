using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;
using Horizon.Server.Modules.References.Domain.Entities;
using Horizon.Server.Modules.Shared.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.ApproveRequest;

/// <summary>
/// Handles <see cref="ApproveRequestCommand"/>: records an approver's positive decision,
/// advances the workflow to the next stage (or marks the request as fully approved),
/// and persists an <see cref="ApprovalHistory"/> entry.
/// </summary>
/// <remarks>
/// Demonstrates the <b>Command pattern</b> (via MediatR) and <b>State pattern</b>:
/// the <see cref="Request"/> aggregate transitions its own status through domain methods,
/// keeping transition logic encapsulated in the entity rather than the handler.
/// </remarks>
public class ApproveRequestCommandHandler : IRequestHandler<ApproveRequestCommand>
{
    private readonly AppDbContext _db;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveRequestCommandHandler> _logger;

    /// <summary>Initialises the handler with required dependencies.</summary>
    public ApproveRequestCommandHandler(AppDbContext db, IUnitOfWork unitOfWork,
        ILogger<ApproveRequestCommandHandler> logger)
    {
        _db = db;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(ApproveRequestCommand command, CancellationToken cancellationToken)
    {
        // 1. Load the request with its approval template and stages.
        var request = await _db.Set<Request>()
            .Include(r => r.ApprovalTemplate)
            .FirstOrDefaultAsync(r => r.Id == command.RequestId, cancellationToken)
            ?? throw new InvalidOperationException($"Request {command.RequestId} not found.");

        // 2. Verify the current stage matches the command.
        if (request.CurrentStageNumber != command.StageNumber)
            throw new InvalidOperationException(
                $"Request {command.RequestId} is at stage {request.CurrentStageNumber}, not {command.StageNumber}.");

        // 3. Determine whether there is a next stage.
        var stages = await _db.Set<ApprovalStage>()
            .Where(s => s.TemplateId == request.ApprovalTemplateId)
            .OrderBy(s => s.StageNumber)
            .ToListAsync(cancellationToken);

        var nextStage = stages.FirstOrDefault(s => s.StageNumber > command.StageNumber);
        bool isFinalApproval = nextStage is null;

        // 4. Look up required status IDs.
        var approvedStatus = await _db.Set<RequestStatus>()
            .FirstOrDefaultAsync(s => s.Code == "APPROVED", cancellationToken)
            ?? throw new InvalidOperationException("Reference data 'APPROVED' status not found.");

        var pendingHrStatus = await _db.Set<RequestStatus>()
            .FirstOrDefaultAsync(s => s.Code == "PENDING_HR", cancellationToken)
            ?? throw new InvalidOperationException("Reference data 'PENDING_HR' status not found.");

        int newStatusId = isFinalApproval ? approvedStatus.Id : pendingHrStatus.Id;

        // 5. Look up APPROVE decision type.
        var approveDecision = await _db.Set<DecisionType>()
            .FirstOrDefaultAsync(d => d.Code == "APPROVE", cancellationToken)
            ?? throw new InvalidOperationException("Reference data 'APPROVE' decision not found.");

        // 6. Apply the domain transition (also raises domain event on final approval).
        request.Approve(newStatusId, nextStage?.StageNumber, isFinalApproval);

        // 7. Persist the approver's history record.
        var history = new ApprovalHistory(
            requestId: request.Id,
            stageNumber: command.StageNumber,
            approverId: command.ApproverId,
            decisionId: approveDecision.Id,
            comment: command.Comment,
            nextStageNumber: nextStage?.StageNumber,
            decisionDate: DateTime.UtcNow,
            createdBy: command.ApproverId);

        await _db.Set<ApprovalHistory>().AddAsync(history, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Request {RequestId} approved at stage {Stage} by approver {ApproverId}. IsFinal={IsFinal}",
            request.Id, command.StageNumber, command.ApproverId, isFinalApproval);
    }
}
