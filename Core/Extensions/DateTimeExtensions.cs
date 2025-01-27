namespace Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime RemoveSeconds(this DateTime dateTime) => dateTime
        .AddSeconds(-dateTime.Second)
        .AddMilliseconds(-dateTime.Millisecond)
        .AddMicroseconds(-dateTime.Microsecond)
    ;
}