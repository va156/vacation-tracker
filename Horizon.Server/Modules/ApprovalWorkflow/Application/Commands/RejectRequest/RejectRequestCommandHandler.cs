using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;
using Horizon.Server.Modules.References.Domain.Entities;
using Horizon.Server.Modules.Shared.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.RejectRequest;

/// <summary>
/// Handles <see cref="RejectRequestCommand"/>: records the rejection decision, moves the request
/// to REJECTED status and persists the <see cref="ApprovalHistory"/> entry.
/// </summary>
/// <remarks>
/// Demonstrates the <b>State pattern</b> — the <see cref="Request"/> aggregate owns the
/// transition logic; this handler only resolves IDs and persists the outcome.
/// </remarks>
public class RejectRequestCommandHandler : IRequestHandler<RejectRequestCommand>
{
    private readonly AppDbContext _db;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectRequestCommandHandler> _logger;

    /// <summary>Initialises the handler with required dependencies.</summary>
    public RejectRequestCommandHandler(AppDbContext db, IUnitOfWork unitOfWork,
        ILogger<RejectRequestCommandHandler> logger)
    {
        _db = db;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(RejectRequestCommand command, CancellationToken cancellationToken)
    {
        var request = await _db.Set<Request>()
            .FirstOrDefaultAsync(r => r.Id == command.RequestId, cancellationToken)
            ?? throw new InvalidOperationException($"Request {command.RequestId} not found.");

        if (request.CurrentStageNumber != command.StageNumber)
            throw new InvalidOperationException(
                $"Request {command.RequestId} is at stage {request.CurrentStageNumber}, not {command.StageNumber}.");

        var rejectedStatus = await _db.Set<RequestStatus>()
            .FirstOrDefaultAsync(s => s.Code == "REJECTED", cancellationToken)
            ?? throw new InvalidOperationException("Reference data 'REJECTED' status not found.");

        var rejectDecision = await _db.Set<DecisionType>()
            .FirstOrDefaultAsync(d => d.Code == "REJECT", cancellationToken)
            ?? throw new InvalidOperationException("Reference data 'REJECT' decision not found.");

        // Domain transition — raises RequestRejectedEvent.
        request.Reject(rejectedStatus.Id, command.Comment);

        var history = new ApprovalHistory(
            requestId: request.Id,
            stageNumber: command.StageNumber,
            approverId: command.ApproverId,
            decisionId: rejectDecision.Id,
            comment: command.Comment,
            nextStageNumber: null,
            decisionDate: DateTime.UtcNow,
            createdBy: command.ApproverId);

        await _db.Set<ApprovalHistory>().AddAsync(history, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Request {RequestId} rejected at stage {Stage} by approver {ApproverId}.",
            request.Id, command.StageNumber, command.ApproverId);
    }
}
