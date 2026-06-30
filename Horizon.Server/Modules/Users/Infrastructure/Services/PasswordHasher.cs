using Horizon.Server.Modules.Users.Domain.Interfaces;

namespace Horizon.Server.Modules.Users.Infrastructure.Services;

/// <summary>
/// BCrypt-based implementation of <see cref="IPasswordHasher"/>.
/// Uses a work factor of 12 (2¹² iterations), which balances security against
/// login latency on typical server hardware.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    /// <inheritdoc />
    /// <remarks>
    /// A unique salt is generated automatically by BCrypt and embedded in the returned hash string,
    /// so no separate salt storage is required.
    /// </remarks>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    /// <inheritdoc />
    /// <remarks>
    /// BCrypt extracts the embedded salt from <paramref name="passwordHash"/> before comparing,
    /// making timing attacks and rainbow-table attacks impractical.
    /// </remarks>
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
