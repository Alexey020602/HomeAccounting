using Shared.Model;

namespace Reports.Contracts;

public interface IReportUseCase
{
    Task<ReportDto> GetReport(ReportRequest request);
}
