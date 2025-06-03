using Core.Model.Report;

namespace Core.Services;

public interface IReportUseCase
{
    Task<Report> GetReport(ReportRequest request);
}