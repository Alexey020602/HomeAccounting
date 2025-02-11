using Core.Model.Report;
using Refit;

namespace Client.Api;

public interface IReportsApi
{
    [Get("/reports/month")]
    Task<Report> GetMonthReport(ReportRequest request);
}