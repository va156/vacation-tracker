using Horizon.Server.Modules.Users.Domain.Entities;

namespace Horizon.Server.Modules.Users.Domain.Interfaces;

/// <summary>
/// Generates and validates JWT tokens for user authentication.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Creates a short-lived JWT access token containing the user's identity,
    /// role, and permission claims.
    /// </summary>
    /// <param name="user">The authenticated user (must have <c>Role</c> navigation property loaded).</param>
    /// <returns>A signed JWT string ready to be returned in the authentication response.</returns>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Generates a cryptographically random opaque refresh token string.
    /// The caller is responsible for persisting the token and associating it with the user.
    /// </summary>
    /// <returns>A base-64 encoded 32-byte random string.</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates the signature and claims of an expired JWT and returns the contained
    /// <see cref="System.Security.Claims.ClaimsPrincipal"/>. Lifetime validation is intentionally
    /// skipped so that expired tokens can be used to identify the user during token rotation.
    /// </summary>
    /// <param name="token">An expired (or valid) JWT string.</param>
    /// <returns>The claims principal, or <c>null</c> if the token is structurally invalid.</returns>
    System.Security.Claims.ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
