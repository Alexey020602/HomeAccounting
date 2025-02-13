using Core.Model.Report;
using Refit;

namespace BlazorShared.Api;

public interface IReportsApi
{
    [Get("/reports/month")]
    Task<Report> GetMonthReport(ReportRequest request);
}