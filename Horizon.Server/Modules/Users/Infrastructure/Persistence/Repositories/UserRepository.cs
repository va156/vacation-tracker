using Horizon.Server.Modules.Users.Domain.Entities;
using Horizon.Server.Modules.Users.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.Users.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Set<User>()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Set<User>()
            .Include(u => u.Role)
            .ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Set<User>().AddAsync(user);
    }

    public Task UpdateAsync(User user)
    {
        _context.Set<User>().Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user)
    {
        _context.Set<User>().Remove(user);
        return Task.CompletedTask;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Set<User>()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Set<User>()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.Set<User>()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Set<User>().AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _context.Set<User>().AnyAsync(u => u.Username == username);
    }
    public async Task<Role?> GetRoleByCodeAsync(string code)
    {
        return await _context.Set<Role>()
            .FirstOrDefaultAsync(r => r.Code == code);
    }
    // Методы для refresh токенов
    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.Set<RefreshToken>()
            .Include(rt => rt.User)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveRefreshTokensAsync(int userId)
    {
        return await _context.Set<RefreshToken>()
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _context.Set<RefreshToken>().AddAsync(refreshToken);
    }

    public async Task RevokeAllUserRefreshTokensAsync(int userId, string ipAddress)
    {
        var activeTokens = await _context.Set<RefreshToken>()
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();

        foreach (var token in activeTokens)
        {
            token.Revoke(ipAddress);
        }
    }

    public async Task<(IEnumerable<User> Users, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null)
    {
        var query = _context.Set<User>()
            .Include(u => u.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u =>
                u.Username.Contains(searchTerm) ||
                u.Email.Contains(searchTerm) ||
                u.FirstName.Contains(searchTerm) ||
                u.LastName.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }
}