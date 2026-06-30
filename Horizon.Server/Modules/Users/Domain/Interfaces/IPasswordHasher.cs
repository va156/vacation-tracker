namespace Horizon.Server.Modules.Users.Domain.Interfaces;

/// <summary>
/// Abstracts password hashing so that the algorithm (currently BCrypt) can be swapped
/// without touching domain or application code.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Produces a one-way hash of the plain-text password suitable for persistent storage.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>A self-contained hash string that includes the salt and work-factor.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies that <paramref name="password"/> matches the stored <paramref name="passwordHash"/>.
    /// The salt and work-factor are extracted from the hash itself.
    /// </summary>
    /// <param name="password">The plain-text password provided by the user.</param>
    /// <param name="passwordHash">The stored hash to compare against.</param>
    /// <returns><c>true</c> when the password matches the hash; otherwise <c>false</c>.</returns>
    bool VerifyPassword(string password, string passwordHash);
}
