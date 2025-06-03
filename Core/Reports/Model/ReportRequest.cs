namespace Core.Model.Report;

public record ReportRequest(DateRange Range, string? Login = null)
{
    // public ReportRequest(Months month) : this(month, DateTimeExtensions.GetCurrentYear())
    // {
    // }
}