using Core.Extensions;

namespace Core.Model.Report;

public record ReportRequest(int Month, int Year)
{
    // public ReportRequest(Months month) : this(month, DateTimeExtensions.GetCurrentYear())
    // {
    // }
}