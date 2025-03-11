using BlazorShared.Api.Attributes;
using Core.Model.Report;
using Refit;

namespace BlazorShared.Api;

[ApiAuthorizable("reports/month")]
[Headers("Authorization: Bearer")]
public interface IReportsApi
{
    [Put("/")]
    Task<Report> GetMonthReport(ReportRequest request);
}