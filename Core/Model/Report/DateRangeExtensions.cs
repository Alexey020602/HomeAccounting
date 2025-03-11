namespace Core.Model.Report;

public static class DateRangeExtensions
{
    public static bool IsSameMonth(this DateRange dateRange) => dateRange.Start.Month == dateRange.End.Month;
    public static bool IsSameYear(this DateRange dateRange) => dateRange.Start.Year == dateRange.End.Year;
    
    
}