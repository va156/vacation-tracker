using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;
using Horizon.Server.Modules.References.Domain.Entities;
using Horizon.Server.Modules.Shared.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.SendBackRequest;

/// <summary>
/// Handles <see cref="SendBackRequestCommand"/>: returns a leave request to the employee
/// for revision, sets SENT_BACK status and records the decision in <see cref="ApprovalHistory"/>.
/// </summary>
public class SendBackRequestCommandHandler : IRequestHandler<SendBackRequestCommand>
{
    private readonly AppDbContext _db;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SendBackRequestCommandHandler> _logger;

    /// <summary>Initialises the handler with required dependencies.</summary>
    public SendBackRequestCommandHandler(AppDbContext db, IUnitOfWork unitOfWork,
        ILogger<SendBackRequestCommandHandler> logger)
    {
        _db = db;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(SendBackRequestCommand command, CancellationToken cancellationToken)
    {
        var request = await _db.Set<Request>()
            .FirstOrDefaultAsync(r => r.Id == command.RequestId, cancellationToken)
            ?? throw new InvalidOperationException($"Request {command.RequestId} not found.");

        if (request.CurrentStageNumber != command.StageNumber)
            throw new InvalidOperationException(
                $"Request {command.RequestId} is at stage {request.CurrentStageNumber}, not {command.StageNumber}.");

        var sentBackStatus = await _db.Set<RequestStatus>()
            .FirstOrDefaultAsync(s => s.Code == "SENT_BACK", cancellationToken)
            ?? throw new InvalidOperationException("Reference data 'SENT_BACK' status not found.");

        var sendBackDecision = await _db.Set<DecisionType>()
            .FirstOrDefaultAsync(d => d.Code == "SEND_BACK", cancellationToken)
            ?? throw new InvalidOperationException("Reference data 'SEND_BACK' decision not found.");

        // Domain transition — raises RequestSentBackEvent.
        request.SendBack(sentBackStatus.Id, command.Comment);

        var history = new ApprovalHistory(
            requestId: request.Id,
            stageNumber: command.StageNumber,
            approverId: command.ApproverId,
            decisionId: sendBackDecision.Id,
            comment: command.Comment,
            nextStageNumber: null,
            decisionDate: DateTime.UtcNow,
            createdBy: command.ApproverId);

        await _db.Set<ApprovalHistory>().AddAsync(history, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Request {RequestId} sent back at stage {Stage} by approver {ApproverId}.",
            request.Id, command.StageNumber, command.ApproverId);
    }
}
