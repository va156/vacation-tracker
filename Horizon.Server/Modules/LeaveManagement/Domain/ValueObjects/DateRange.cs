using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.LeaveManagement.Domain.ValueObjects;

/// <summary>
/// Value object that represents a validated, inclusive date range for a leave period.
/// Equality is based on <see cref="StartDate"/> and <see cref="EndDate"/> only;
/// <see cref="DurationDays"/> is computed and not part of the identity.
/// </summary>
public class DateRange : ValueObject
{
    /// <summary>Gets the first day of the range (inclusive), normalised to midnight.</summary>
    public DateTime StartDate { get; }

    /// <summary>Gets the last day of the range (inclusive), normalised to midnight.</summary>
    public DateTime EndDate { get; }

    /// <summary>Gets the total number of calendar days covered by the range (inclusive at both ends).</summary>
    public int DurationDays => (EndDate - StartDate).Days + 1;

    /// <summary>
    /// Creates a new date range.
    /// </summary>
    /// <param name="startDate">First day (must be today or in the future).</param>
    /// <param name="endDate">Last day (must be ≥ <paramref name="startDate"/>).</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="startDate"/> is after <paramref name="endDate"/>,
    /// or when <paramref name="startDate"/> is in the past.
    /// </exception>
    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be later than end date");

        if (startDate < DateTime.Today)
            throw new ArgumentException("Cannot create a leave in the past");

        StartDate = startDate.Date;
        EndDate = endDate.Date;
    }

    /// <summary>Returns <c>true</c> when this range overlaps with <paramref name="other"/>.</summary>
    public bool OverlapsWith(DateRange other)
    {
        return StartDate <= other.EndDate && other.StartDate <= EndDate;
    }

    /// <summary>Returns <c>true</c> when <paramref name="date"/> falls within this range (inclusive).</summary>
    public bool Contains(DateTime date)
    {
        return date.Date >= StartDate && date.Date <= EndDate;
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
