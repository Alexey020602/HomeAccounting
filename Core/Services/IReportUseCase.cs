using Core.Model.Report;
using Check = Core.Model.ChecksList.Check;

namespace Core.Services;

public interface IReportUseCase
{
    public Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100);
    Task<Report> GetReport(ReportRequest request);
}