namespace Shared.Model.Dates;

public static class DateTimeExtensions
{
    public static DateTime RemoveSeconds(this DateTime dateTime)
    {
        return dateTime
            .AddSeconds(-dateTime.Second)
            .AddMilliseconds(-dateTime.Millisecond)
            .AddMicroseconds(-dateTime.Microsecond);
    }

    public static DateTime DayOfCurrentMonth(this DateTime date, int day) => new DateTime(date.Year, date.Month, day);
    public static DateTime DayOfCurrentMonth(int day)
    {
        var now = DateTime.Now;
        return new DateTime(now.Year, now.Month, day);
    }

    public static DateTime BeginOfBillingPerion(this DateTime date, int beginDay = 7) =>
        date.DayOfCurrentMonth(beginDay);
    
    internal static DateTime DayOfMonth(int day, Months.Months month) => new DateTime(DateTime.UtcNow.Year, (int)month, day);
    
    internal static Months.Months GetMonth(this DateTime date) => (Months.Months) date.Month;
    
    
    public static int GetCurrentYear() => DateTime.UtcNow.Year;
}
