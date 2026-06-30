using Microsoft.Extensions.DependencyInjection;
using Horizon.Server.Modules.ApprovalWorkflow.Domain.Interfaces;
using Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Persistence.Repositories;
using Horizon.Server.Modules.ApprovalWorkflow.Public;
using Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Services;

namespace Horizon.Server.Modules.ApprovalWorkflow;

public static class ModuleRegistration
{
    public static IServiceCollection AddApprovalWorkflowModule(this IServiceCollection services)
    {
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IApprovalTemplateRepository, ApprovalTemplateRepository>();
        services.AddScoped<IApprovalHistoryRepository, ApprovalHistoryRepository>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));

        services.AddScoped<ApprovalRoutingService>();
        services.AddScoped<RequestNumberGenerator>();

        return services;
    }
}