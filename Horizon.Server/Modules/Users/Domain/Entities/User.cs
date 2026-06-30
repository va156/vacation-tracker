using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.Users.Domain.Entities;

/// <summary>
/// Represents a system user account. A user may optionally be linked to an
/// <see cref="Employee"/> record via <see cref="EmployeeId"/> when the account belongs
/// to an actual company employee. The class is an aggregate root — all user-related
/// state changes (last login, etc.) go through this entity.
/// </summary>
public class User : BaseEntity, IAggregateRoot
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private User() { }

    /// <summary>
    /// Creates a standalone user account (not yet linked to an employee record).
    /// </summary>
    /// <param name="username">Unique login name.</param>
    /// <param name="email">Unique email address.</param>
    /// <param name="passwordHash">BCrypt hash of the user's password.</param>
    /// <param name="firstName">User's first name.</param>
    /// <param name="lastName">User's last name.</param>
    /// <param name="roleId">FK to the role assigned to this user.</param>
    /// <param name="createdBy">ID of the actor who created the account (0 = system).</param>
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

    /// <summary>
    /// Creates a user account pre-linked to an existing employee record.
    /// </summary>
    /// <param name="username">Unique login name.</param>
    /// <param name="email">Unique email address.</param>
    /// <param name="passwordHash">BCrypt hash of the user's password.</param>
    /// <param name="firstName">User's first name.</param>
    /// <param name="lastName">User's last name.</param>
    /// <param name="roleId">FK to the role assigned to this user.</param>
    /// <param name="employeeId">FK to the linked employee record.</param>
    /// <param name="createdBy">ID of the actor who created the account.</param>
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

    /// <summary>Gets the unique login name.</summary>
    public string Username { get; private set; } = null!;

    /// <summary>Gets the unique email address used for authentication.</summary>
    public string Email { get; private set; } = null!;

    /// <summary>Gets the BCrypt-hashed password. Never expose this value in API responses.</summary>
    public string PasswordHash { get; private set; } = null!;

    /// <summary>Gets the user's first (given) name.</summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>Gets the user's last (family) name.</summary>
    public string LastName { get; private set; } = null!;

    /// <summary>Gets the user's optional patronymic / middle name.</summary>
    public string? MiddleName { get; private set; }

    /// <summary>Gets the user's optional contact phone number.</summary>
    public string? Phone { get; private set; }

    /// <summary>Gets the user's optional date of birth.</summary>
    public DateTime? DateOfBirth { get; private set; }

    /// <summary>Gets the FK of the role currently assigned to this user.</summary>
    public int RoleId { get; private set; }

    /// <summary>Gets the FK of the linked employee record, if any.</summary>
    public int? EmployeeId { get; private set; }

    /// <summary>
    /// Gets <c>true</c> when this user account is linked to an employee record.
    /// </summary>
    public bool IsEmployee { get; private set; }

    /// <summary>Gets the UTC timestamp of the user's most recent successful login, or <c>null</c>.</summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>Navigation property to the assigned role.</summary>
    public virtual Role Role { get; private set; } = null!;

    /// <summary>Records the current UTC time as the last successful login timestamp.</summary>
    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}
