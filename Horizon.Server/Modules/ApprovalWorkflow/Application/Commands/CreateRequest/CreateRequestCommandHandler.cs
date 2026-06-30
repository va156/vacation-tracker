using MediatR;

namespace Horizon.Server.Modules.ApprovalWorkflow.Application.Commands.CreateRequest
{
    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, int>
    {
        public async Task<int> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(1);
        }
    }
}
