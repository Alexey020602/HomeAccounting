using BlazorShared.Api.Attributes;
using Refit;
using Reports.Core;
using ReportRequest = Reports.Dto.ReportRequest;

namespace BlazorShared.Api;

[ApiAuthorizable("reports/month")]
[Headers("Authorization: Bearer")]
public interface IReportsApi
{
    [Put("/")]
    Task<Report> GetMonthReport(ReportRequest request);
}