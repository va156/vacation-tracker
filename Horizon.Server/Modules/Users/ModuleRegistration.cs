using Horizon.Server.Modules.Shared.Application.Interfaces.Services;
using Horizon.Server.Modules.Users.Domain.Interfaces;
using Horizon.Server.Modules.Users.Infrastructure.Persistence.Repositories;
using Horizon.Server.Modules.Users.Infrastructure.Services;
using AuthenticationService = Horizon.Server.Modules.Users.Infrastructure.Services.AuthenticationService;
using IAuthenticationService = Horizon.Server.Modules.Users.Public.IAuthenticationService;

namespace Horizon.Server.Modules.Users;

public static class ModuleRegistration
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}