namespace Horizon.Server.Modules.Shared.Domain.Common
{
    /// <summary>
    /// Contract for reference-data entities (lookup tables).
    /// Implementing entities are identified by a business <see cref="Code"/>, carry a
    /// human-readable <see cref="Name"/>, support ordering via <see cref="SortOrder"/>,
    /// and can be deactivated without physical deletion.
    /// </summary>
    public interface IReferenceEntity
    {
        /// <summary>Gets the unique business code used to identify the reference item in code and config.</summary>
        string Code { get; }

        /// <summary>Gets the display name shown in the UI.</summary>
        string Name { get; }

        /// <summary>Gets the sort position used when rendering lists.</summary>
        int SortOrder { get; }

        /// <summary>Gets whether this reference item is currently active and selectable.</summary>
        bool IsActive { get; }
    }
}
