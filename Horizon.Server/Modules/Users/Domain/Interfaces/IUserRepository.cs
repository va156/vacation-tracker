using Horizon.Server.Modules.Users.Domain.Entities;

namespace Horizon.Server.Modules.Users.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);


    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmployeeIdAsync(int employeeId);
    Task<Role?> GetRoleByCodeAsync(string code);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);

    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task<IEnumerable<RefreshToken>> GetActiveRefreshTokensAsync(int userId);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task RevokeAllUserRefreshTokensAsync(int userId, string ipAddress);

    Task<(IEnumerable<User> Users, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
}