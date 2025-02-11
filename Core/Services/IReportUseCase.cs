using Core.Model.Report;
using Check = Core.Model.ChecksList.Check;

namespace Core.Services;

public interface IReportUseCase
{
    Task<Report> GetReport(ReportRequest request);
}