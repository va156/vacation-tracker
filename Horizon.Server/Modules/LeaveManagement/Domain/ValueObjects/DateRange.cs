using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.LeaveManagement.Domain.ValueObjects;

public class DateRange : ValueObject
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public int DurationDays => (EndDate - StartDate).Days + 1;

    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Дата начала не может быть позже даты окончания");

        if (startDate < DateTime.Today)
            throw new ArgumentException("Нельзя создавать отпуск в прошлом");

        StartDate = startDate.Date;
        EndDate = endDate.Date;
    }

    public bool OverlapsWith(DateRange other)
    {
        return StartDate <= other.EndDate && other.StartDate <= EndDate;
    }

    public bool Contains(DateTime date)
    {
        return date.Date >= StartDate && date.Date <= EndDate;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
