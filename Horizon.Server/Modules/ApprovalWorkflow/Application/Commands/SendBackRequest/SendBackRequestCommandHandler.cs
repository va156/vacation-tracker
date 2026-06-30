using MediatR;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.SendBackRequest
{
    public class SendBackRequestCommandHandler : IRequestHandler<SendBackRequestCommand>
    {
        public async Task Handle(SendBackRequestCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
