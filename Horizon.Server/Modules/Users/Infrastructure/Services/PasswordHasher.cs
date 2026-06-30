using Horizon.Server.Modules.Users.Domain.Interfaces;

namespace Horizon.Server.Modules.Users.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        // 12 означает 2^12 итераций
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    // Проверяет пароль
    // BCrypt извлекает соль из хеша и проверяет
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}