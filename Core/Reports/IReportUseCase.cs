using Core.Model.Report;
using Core.Reports.Model;

namespace Core.Services;

public interface IReportUseCase
{
    Task<Report> GetReport(ReportRequest request);
}