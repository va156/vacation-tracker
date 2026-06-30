namespace Horizon.Server.Modules.Users.Domain.Events
{
    /// <summary>
    /// Raised each time a user successfully authenticates.
    /// Handlers may use this event to record login history, trigger MFA prompts, etc.
    /// </summary>
    public class UserLoggedInEvent
    {
    }
}
