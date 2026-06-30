using MediatR;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.RejectRequest
{
    public class RejectRequestCommandHandler : IRequestHandler<RejectRequestCommand>
    {
        public async Task Handle(RejectRequestCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
