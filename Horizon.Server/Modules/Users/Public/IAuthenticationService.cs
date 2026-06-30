using Horizon.Server.Modules.Users.Application.DTOs;

namespace Horizon.Server.Modules.Users.Public;

/// <summary>
/// Public facade for all authentication operations exposed by the Users module.
/// Consumed by <c>AuthController</c>; backed by <c>AuthenticationService</c>.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Registers a new user account and returns a ready-to-use token pair.
    /// User creation and refresh-token generation are performed in a single database transaction.
    /// </summary>
    /// <param name="request">Registration details (username, email, password, name).</param>
    /// <param name="ipAddress">Client IP address used to record refresh token origin.</param>
    /// <returns>Access token, refresh token, expiry, and user profile.</returns>
    /// <exception cref="InvalidOperationException">Email or username already in use, or the default role is missing.</exception>
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, string ipAddress);

    /// <summary>
    /// Validates credentials and issues a new token pair for the user.
    /// </summary>
    /// <param name="request">Email and password supplied by the client.</param>
    /// <param name="ipAddress">Client IP address.</param>
    /// <returns>Access token, refresh token, expiry, and user profile.</returns>
    /// <exception cref="UnauthorizedAccessException">Credentials are invalid or the account is deactivated.</exception>
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, string ipAddress);

    /// <summary>
    /// Rotates a refresh token: revokes the old one and issues a new access + refresh token pair.
    /// </summary>
    /// <param name="refreshToken">The current refresh token sent by the client.</param>
    /// <param name="ipAddress">Client IP address.</param>
    /// <returns>New access token, new refresh token, expiry, and user profile.</returns>
    /// <exception cref="UnauthorizedAccessException">Token is invalid, expired, or the account is deactivated.</exception>
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress);

    /// <summary>
    /// Revokes a refresh token so it can no longer be used (e.g. during explicit logout).
    /// </summary>
    /// <param name="refreshToken">The refresh token to revoke.</param>
    /// <param name="ipAddress">Client IP address.</param>
    /// <exception cref="UnauthorizedAccessException">Token is not found or already invalid.</exception>
    Task RevokeTokenAsync(string refreshToken, string ipAddress);
}
