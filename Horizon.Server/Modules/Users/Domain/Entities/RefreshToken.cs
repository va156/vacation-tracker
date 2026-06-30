using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Users.Domain.Entities;

/// <summary>
/// Represents a long-lived refresh token issued to a user after authentication.
/// A refresh token can be used exactly once to obtain a new access token and a
/// new refresh token. Once revoked (manually or during rotation), it cannot be reused.
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private RefreshToken() { }

    /// <summary>
    /// Creates a new refresh token record.
    /// </summary>
    /// <param name="token">Cryptographically random base-64 token string.</param>
    /// <param name="userId">FK of the user this token was issued to.</param>
    /// <param name="expiresAt">UTC expiry timestamp.</param>
    /// <param name="createdByIp">IP address from which the token was requested.</param>
    public RefreshToken(string token, int userId, DateTime expiresAt, string createdByIp)
    {
        Token = token;
        UserId = userId;
        ExpiresAt = expiresAt;
        CreatedByIp = createdByIp;
        IsRevoked = false;
    }

    /// <summary>Gets the opaque token string sent to the client.</summary>
    public string Token { get; private set; } = null!;

    /// <summary>Gets the FK of the user this token was issued to.</summary>
    public int UserId { get; private set; }

    /// <summary>Gets the UTC timestamp after which the token is no longer valid.</summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>Gets the IP address from which the token was originally created.</summary>
    public string CreatedByIp { get; private set; } = null!;

    /// <summary>Gets the UTC timestamp when the token was revoked, or <c>null</c> if still active.</summary>
    public DateTime? RevokedAt { get; private set; }

    /// <summary>Gets the IP address from which the revocation was performed, or <c>null</c>.</summary>
    public string? RevokedByIp { get; private set; }

    /// <summary>Gets <c>true</c> when the token has been explicitly revoked.</summary>
    public bool IsRevoked { get; private set; }

    /// <summary>Navigation property to the owning user.</summary>
    public virtual User User { get; private set; } = null!;

    /// <summary>
    /// Revokes the token, preventing any future use.
    /// </summary>
    /// <param name="revokedByIp">IP address initiating the revocation.</param>
    public void Revoke(string revokedByIp)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = revokedByIp;
    }

    /// <summary>
    /// Gets <c>true</c> when the token has not been revoked and has not yet expired.
    /// </summary>
    public bool IsTokenValid => !IsRevoked && ExpiresAt > DateTime.UtcNow;
}
