using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.References.Domain.Entities;

/// <summary>
/// Reference entity classifying a type of leave (e.g. Annual, Sick, Unpaid).
/// Controls whether the leave is compensated (<see cref="IsPaid"/>), whether it
/// reduces the employee's balance (<see cref="AffectsBalance"/>), and the
/// permitted duration range (<see cref="MinDays"/> / <see cref="MaxDays"/>).
/// The optional <see cref="AccrualRate"/> defines how many days per month the
/// employee earns for this leave type.
/// </summary>
public class LeaveType : BaseEntity, IReferenceEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private LeaveType() { }

    /// <summary>
    /// Creates a new leave type.
    /// </summary>
    /// <param name="code">Unique business code (e.g. "ANNUAL", "SICK").</param>
    /// <param name="name">Display name shown in the UI.</param>
    /// <param name="isPaid"><c>true</c> if the employee receives pay during this leave type.</param>
    /// <param name="affectsBalance"><c>true</c> if taking this leave deducts from the balance.</param>
    /// <param name="minDays">Minimum number of days allowed per single request.</param>
    /// <param name="maxDays">Maximum number of days allowed per single request.</param>
    /// <param name="sortOrder">Display order in leave type lists.</param>
    /// <param name="createdBy">ID of the user creating this record.</param>
    public LeaveType(string code, string name, bool isPaid, bool affectsBalance,
        int minDays, int maxDays, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        IsPaid = isPaid;
        AffectsBalance = affectsBalance;
        MinDays = minDays;
        MaxDays = maxDays;
        SortOrder = sortOrder;
    }

    /// <summary>Gets the unique business code.</summary>
    public string Code { get; private set; } = null!;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = null!;

    /// <summary>Gets <c>true</c> when the employee is compensated during this leave type.</summary>
    public bool IsPaid { get; private set; }

    /// <summary>Gets <c>true</c> when taking this leave reduces the employee's balance.</summary>
    public bool AffectsBalance { get; private set; }

    /// <summary>Gets the minimum number of days per single leave request.</summary>
    public int MinDays { get; private set; }

    /// <summary>Gets the maximum number of days per single leave request.</summary>
    public int MaxDays { get; private set; }

    /// <summary>Gets the optional monthly accrual rate in days, or <c>null</c> if the leave is not accrued.</summary>
    public decimal? AccrualRate { get; private set; }

    /// <summary>Gets the display sort order.</summary>
    public int SortOrder { get; private set; }
}
