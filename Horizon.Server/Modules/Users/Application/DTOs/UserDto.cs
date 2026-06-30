namespace Horizon.Server.Modules.Users.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? Phone { get; set; }
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public int? EmployeeId { get; set; }
    public bool IsEmployee { get; set; }
    public DateTime? LastLoginAt { get; set; }
}