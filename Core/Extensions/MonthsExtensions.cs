using Core.Model;

namespace Core.Extensions;

public static class MonthsExtensions
{
    public static Months GetMonths(this DateTime dateTime) => (Months)dateTime.Month;
    
}