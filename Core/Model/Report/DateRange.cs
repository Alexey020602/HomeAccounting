using System.Text.Json.Serialization;

namespace Core.Model.Report;

public struct DateRange
{
    public static DateRange CreateMonthRange(int firstDay, int month, int year)
    {
        // if (year is < 1 or > 9999) throw new ArgumentOutOfRangeException(nameof(year), year, "Year must be between 1 and 9999");
        // if (month is < 1 or > 12) throw new ArgumentOutOfRangeException(nameof(month), month, "Month must be between 1 and 12");
        var startDate = new DateTime(year, month, firstDay);
        return new DateRange
        {
            Start = startDate,
            End = startDate.AddMonths(1),
        };
    }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}