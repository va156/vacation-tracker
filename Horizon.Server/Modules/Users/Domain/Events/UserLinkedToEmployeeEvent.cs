namespace Horizon.Server.Modules.Users.Domain.Events
{
    /// <summary>
    /// Raised when an existing user account is associated with an employee record.
    /// Handlers may use this event to synchronise HR data, update role assignments, etc.
    /// </summary>
    public class UserLinkedToEmployeeEvent
    {
    }
}
