using Microsoft.Extensions.DependencyInjection;
using Horizon.Server.Modules.References.Domain.Interfaces;
using Horizon.Server.Modules.References.Infrastructure.Persistence.Repositories;
using Horizon.Server.Modules.References.Public;

namespace Horizon.Server.Modules.References;

public static class ModuleRegistration
{
    public static IServiceCollection AddReferencesModule(this IServiceCollection services)
    {
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<IOperationTypeRepository, OperationTypeRepository>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));

        return services;
    }
}