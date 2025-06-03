using Core.Reports.Model;

namespace Reports.Core;

public interface IReportUseCase
{
    Task<Report> GetReport(ReportRequest request);
}

public sealed record ReportRequest(DateRange Range, string? Login = null);