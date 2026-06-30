using MediatR;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.ApproveRequest
{
    public class ApproveRequestCommandHandler : IRequestHandler<ApproveRequestCommand>
    {
        public async Task Handle(ApproveRequestCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
