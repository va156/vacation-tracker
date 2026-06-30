using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Users.Domain.Entities;
public class User : BaseEntity, IAggregateRoot
{
    private User() { }

    public User(string username, string email, string passwordHash, string firstName,
        string lastName, int roleId, int createdBy) : base(createdBy)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash; 
        FirstName = firstName;
        LastName = lastName;
        RoleId = roleId;
        IsEmployee = false;
    }

    public User(string username, string email, string passwordHash, string firstName,
        string lastName, int roleId, int employeeId, int createdBy) : base(createdBy)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;  
        FirstName = firstName;
        LastName = lastName;
        RoleId = roleId;
        EmployeeId = employeeId;
        IsEmployee = true;
    }

    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;  
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string? MiddleName { get; private set; }
    public string? Phone { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public int RoleId { get; private set; }
    public int? EmployeeId { get; private set; }
    public bool IsEmployee { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public virtual Role Role { get; private set; } = null!;

    // Метод для обновления времени последнего входа
    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}