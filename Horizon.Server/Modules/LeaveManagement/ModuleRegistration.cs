using Microsoft.Extensions.DependencyInjection;
using Horizon.Server.Modules.LeaveManagement.Domain.Interfaces;
using Horizon.Server.Modules.LeaveManagement.Infrastructure.Persistence.Repositories;
using Horizon.Server.Modules.LeaveManagement.Public;
using Horizon.Server.Modules.LeaveManagement.Infrastructure.Services;

namespace Horizon.Server.Modules.LeaveManagement;

public static class ModuleRegistration
{
    public static IServiceCollection AddLeaveManagementModule(this IServiceCollection services)
    {
        services.AddScoped<ILeaveRepository, LeaveRepository>();
        services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();
        services.AddScoped<IBalanceTransactionRepository, BalanceTransactionRepository>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));

        services.AddScoped<LeaveCalculationService>();
        services.AddScoped<BalanceValidationService>();

        return services;
    }
}