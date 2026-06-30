using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Users.Domain.Entities;

public class RefreshToken : BaseEntity
{
    private RefreshToken() { }

    public RefreshToken(string token, int userId, DateTime expiresAt, string createdByIp)
    {
        Token = token;
        UserId = userId;
        ExpiresAt = expiresAt;
        CreatedByIp = createdByIp;
        IsRevoked = false;
    }

    public string Token { get; private set; } = null!;        
    public int UserId { get; private set; }                    // Кому принадлежит
    public DateTime ExpiresAt { get; private set; }            // Когда истекает
    public string CreatedByIp { get; private set; } = null!;   // С какого IP создан
    public DateTime? RevokedAt { get; private set; }           // Когда отозван (если отозван)
    public string? RevokedByIp { get; private set; }           // С какого IP отозван
    public bool IsRevoked { get; private set; }                // Флаг отзыва

    public virtual User User { get; private set; } = null!;

    public void Revoke(string revokedByIp)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = revokedByIp;
    }

    public bool IsActive => !IsRevoked && ExpiresAt > DateTime.UtcNow;
}