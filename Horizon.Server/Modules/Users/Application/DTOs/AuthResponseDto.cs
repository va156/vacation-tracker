namespace Horizon.Server.Modules.Users.Application.DTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;      // JWT токен для доступа к API
    public string RefreshToken { get; set; } = string.Empty;     // Токен для обновления access token
    public DateTime ExpiresAt { get; set; }                      // Когда истекает access token
    public UserDto User { get; set; } = null!;                   // Информация о пользователе
}