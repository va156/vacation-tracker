namespace Horizon.Server.Modules.Users.Application.DTOs;

/// <summary>
/// Payload returned to the client after a successful authentication operation
/// (register, login, or token refresh). The refresh token is additionally stored
/// in an HTTP-only cookie by the controller.
/// </summary>
public class AuthResponseDto
{
    /// <summary>Gets or sets the short-lived JWT access token used to authorise API requests.</summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the long-lived refresh token used to obtain a new access token
    /// without re-entering credentials.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>Gets or sets the UTC timestamp when the access token expires.</summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>Gets or sets the profile of the authenticated user.</summary>
    public UserDto User { get; set; } = null!;
}
