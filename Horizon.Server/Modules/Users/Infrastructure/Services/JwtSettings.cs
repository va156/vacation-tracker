namespace Horizon.Server.Modules.Users.Infrastructure.Services;

/// <summary>
/// Strongly-typed configuration model for JWT token settings.
/// Bound from the <c>JwtSettings</c> section of <c>appsettings.json</c>
/// (and overridable via environment variables such as <c>JwtSettings__SecretKey</c>).
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Gets or sets the HMAC-SHA256 signing secret.
    /// Must be at least 32 characters long and must not use the development default in production.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token issuer — typically the API base URL (e.g. <c>https://api.example.com</c>).
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the intended audience — typically the frontend URL (e.g. <c>https://app.example.com</c>).
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets how long (in minutes) an access token remains valid before it must be refreshed.
    /// Recommended range: 15–60 minutes.
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; }

    /// <summary>
    /// Gets or sets how long (in days) a refresh token remains valid before it expires.
    /// Recommended range: 7–30 days.
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; }
}
