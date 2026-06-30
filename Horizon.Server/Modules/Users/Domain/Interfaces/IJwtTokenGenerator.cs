using Horizon.Server.Modules.Users.Domain.Entities;

namespace Horizon.Server.Modules.Users.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user);

    string GenerateRefreshToken();

    /// <summary>
    /// Получает principal из истекшего токена (для refresh)
    /// </summary>
    System.Security.Claims.ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}