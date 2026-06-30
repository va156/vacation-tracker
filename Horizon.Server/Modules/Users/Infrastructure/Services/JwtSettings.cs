namespace Horizon.Server.Modules.Users.Infrastructure.Services;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;     // Секретный ключ для подписи токена
    public string Issuer { get; set; } = string.Empty;        // Кто выпустил токен (обычно URL API)
    public string Audience { get; set; } = string.Empty;      // Для кого предназначен токен (обычно URL клиента)
    public int AccessTokenExpirationMinutes { get; set; }     // Сколько живет access token (обычно 15-60 мин)
    public int RefreshTokenExpirationDays { get; set; }       // Сколько живет refresh token (обычно 7-30 дней)
}