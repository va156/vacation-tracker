using System.ComponentModel.DataAnnotations;

namespace Horizon.Server.Modules.Users.Application.DTOs;

/// <summary>
/// Request payload for new user registration.
/// Validated at the controller layer by both DataAnnotations and
/// <c>RegisterRequestDtoValidator</c> (FluentValidation).
/// </summary>
public class RegisterRequestDto
{
    /// <summary>Gets or sets the desired unique login name (3–50 characters, alphanumeric + underscore).</summary>
    [Required(ErrorMessage = "Username is required")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
    public string Username { get; set; } = string.Empty;

    /// <summary>Gets or sets the email address used for authentication.</summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the plain-text password (minimum 8 characters, at least one uppercase letter and one digit).</summary>
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's first name.</summary>
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's last name.</summary>
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>Gets or sets the optional patronymic / middle name.</summary>
    public string? MiddleName { get; set; }

    /// <summary>Gets or sets the optional contact phone number (E.164 or local format).</summary>
    public string? Phone { get; set; }
}

/// <summary>
/// Request payload for user login.
/// Validated at the controller layer by <c>LoginRequestDtoValidator</c> (FluentValidation).
/// </summary>
public class LoginRequestDto
{
    /// <summary>Gets or sets the email address to authenticate with.</summary>
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the plain-text password to verify.</summary>
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>Request payload for exchanging a refresh token for a new token pair.</summary>
public class RefreshTokenRequestDto
{
    /// <summary>Gets or sets the refresh token string previously issued by the server.</summary>
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>Request payload for explicit token revocation (e.g. logout from another device).</summary>
public class RevokeTokenRequestDto
{
    /// <summary>Gets or sets the refresh token to revoke.</summary>
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
