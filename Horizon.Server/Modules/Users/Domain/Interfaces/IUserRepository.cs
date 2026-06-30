using Horizon.Server.Modules.Users.Domain.Entities;

namespace Horizon.Server.Modules.Users.Domain.Interfaces;

/// <summary>
/// Data-access contract for <see cref="User"/> and related authentication entities.
/// Provides CRUD operations, lookup queries, and refresh-token management.
/// </summary>
public interface IUserRepository
{
    /// <summary>Returns the user with the given primary key, including its role, or <c>null</c>.</summary>
    Task<User?> GetByIdAsync(int id);

    /// <summary>Returns all active (non-soft-deleted) users with their roles.</summary>
    Task<IEnumerable<User>> GetAllAsync();

    /// <summary>Adds a new user to the repository (call <c>SaveChangesAsync</c> to persist).</summary>
    Task AddAsync(User user);

    /// <summary>Marks a user as modified in the change tracker (call <c>SaveChangesAsync</c> to persist).</summary>
    Task UpdateAsync(User user);

    /// <summary>Removes a user from the repository (hard delete — prefer soft-delete via <c>User.Delete</c>).</summary>
    Task DeleteAsync(User user);

    // ── Lookup queries ──────────────────────────────────────────────────────────

    /// <summary>Returns the user whose email matches <paramref name="email"/> (case-insensitive), or <c>null</c>.</summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>Returns the user whose username matches <paramref name="username"/>, or <c>null</c>.</summary>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>Returns the user linked to the given employee record, or <c>null</c>.</summary>
    Task<User?> GetByEmployeeIdAsync(int employeeId);

    /// <summary>Returns the role whose code matches <paramref name="code"/>, or <c>null</c>.</summary>
    Task<Role?> GetRoleByCodeAsync(string code);

    /// <summary>Returns <c>true</c> when a user with the given email already exists.</summary>
    Task<bool> ExistsByEmailAsync(string email);

    /// <summary>Returns <c>true</c> when a user with the given username already exists.</summary>
    Task<bool> ExistsByUsernameAsync(string username);

    // ── Refresh token management ────────────────────────────────────────────────

    /// <summary>Returns the refresh token matching the opaque string, including its user, or <c>null</c>.</summary>
    Task<RefreshToken?> GetRefreshTokenAsync(string token);

    /// <summary>Returns all non-expired, non-revoked refresh tokens for the specified user.</summary>
    Task<IEnumerable<RefreshToken>> GetActiveRefreshTokensAsync(int userId);

    /// <summary>Persists a newly generated refresh token (call <c>SaveChangesAsync</c> to flush).</summary>
    Task AddRefreshTokenAsync(RefreshToken refreshToken);

    /// <summary>Revokes every active refresh token for the user — used during logout or security events.</summary>
    Task RevokeAllUserRefreshTokensAsync(int userId, string ipAddress);

    // ── Pagination ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns a paged subset of active users, optionally filtered by a search term.
    /// </summary>
    /// <param name="pageNumber">1-based page index.</param>
    /// <param name="pageSize">Number of records per page.</param>
    /// <param name="searchTerm">Optional substring to match against username, email, or name fields.</param>
    /// <returns>The page of users and the total record count before paging.</returns>
    Task<(IEnumerable<User> Users, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
}
