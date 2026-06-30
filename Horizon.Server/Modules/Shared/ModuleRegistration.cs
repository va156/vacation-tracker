using Horizon.Server.Modules.Shared.Application.Interfaces.Services;
using Horizon.Server.Modules.Shared.Domain.Abstractions;
using Horizon.Server.Modules.Shared.Infrastructure.Persistence;
using Horizon.Server.Modules.Shared.Infrastructure.Services;

namespace Horizon.Server.Modules.Shared;

public static class ModuleRegistration
{
    public static IServiceCollection AddSharedModule(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}