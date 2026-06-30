namespace Horizon.Server.Modules.Shared.Domain.Abstractions
{
    /// <summary>
    /// Marker interface for all repository types. Concrete repository interfaces
    /// extend this to participate in dependency-injection discovery and provide
    /// a common base for cross-cutting concerns such as logging and caching.
    /// </summary>
    public interface IRepository
    {
    }
}
