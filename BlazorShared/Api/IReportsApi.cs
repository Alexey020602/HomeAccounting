using BlazorShared.Api.Attributes;
using Core.Model.Report;
using Refit;

namespace BlazorShared.Api;

[Api]
public interface IReportsApi
{
    [Get("/reports/month")]
    Task<Report> GetMonthReport(ReportRequest request);
}