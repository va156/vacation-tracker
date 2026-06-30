namespace Horizon.Server.Modules.Users.Application.DTOs;

/// <summary>
/// Read-only projection of a <c>User</c> entity safe for inclusion in API responses.
/// Sensitive fields such as <c>PasswordHash</c> are intentionally excluded.
/// </summary>
public class UserDto
{
    /// <summary>Gets or sets the user's surrogate primary key.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the unique login name.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the first (given) name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the last (family) name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Gets or sets the optional patronymic / middle name.</summary>
    public string? MiddleName { get; set; }

    /// <summary>Gets or sets the optional contact phone number.</summary>
    public string? Phone { get; set; }

    /// <summary>Gets or sets the FK of the user's role.</summary>
    public int RoleId { get; set; }

    /// <summary>Gets or sets the display name of the user's role.</summary>
    public string? RoleName { get; set; }

    /// <summary>Gets or sets the FK of the linked employee record, or <c>null</c>.</summary>
    public int? EmployeeId { get; set; }

    /// <summary>Gets or sets whether this user account is linked to an employee record.</summary>
    public bool IsEmployee { get; set; }

    /// <summary>Gets or sets the UTC timestamp of the user's last successful login, or <c>null</c>.</summary>
    public DateTime? LastLoginAt { get; set; }
}
