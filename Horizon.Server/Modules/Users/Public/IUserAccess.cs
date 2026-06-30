namespace Horizon.Server.Modules.Users.Public
{
    /// <summary>
    /// Public contract for querying user data from other modules.
    /// Prevents direct cross-module access to internal repositories and keeps
    /// module boundaries explicit. To be implemented when cross-module user queries are needed.
    /// </summary>
    public interface IUserAccess
    {
    }
}
