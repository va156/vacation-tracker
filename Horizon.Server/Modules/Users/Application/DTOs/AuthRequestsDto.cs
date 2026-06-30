using System.ComponentModel.DataAnnotations;

namespace Horizon.Server.Modules.Users.Application.DTOs;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "Имя пользователя обязательно")]
    [MinLength(3, ErrorMessage = "Имя пользователя должно содержать минимум 3 символа")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Неверный формат email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имя обязательно")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилия обязательна")]
    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public string? Phone { get; set; }
}

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email обязателен")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; } = string.Empty;
}

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

public class RevokeTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}