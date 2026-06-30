using Horizon.Server.Modules.Shared.Application.Interfaces.Services;

namespace Horizon.Server.Modules.Users.Infrastructure.Services
{
    /// <summary>
    /// Provides access to the currently authenticated user's identity.
    /// Intended to be injected into application/domain services that need to know
    /// who is performing an operation without taking a direct dependency on
    /// <c>IHttpContextAccessor</c>. Implementation reads claims from the ASP.NET Core
    /// HTTP context when the service is wired up.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
    }
}
