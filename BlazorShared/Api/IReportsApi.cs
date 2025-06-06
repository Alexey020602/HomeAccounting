using BlazorShared.Api.Attributes;
using Refit;
using Reports.Contracts;
using ReportRequest = Reports.Contracts.ReportRequest;

namespace BlazorShared.Api;

[ApiAuthorizable("reports/month")]
[Headers("Authorization: Bearer")]
public interface IReportsApi
{
    [Put("/")]
    Task<ReportDto> GetMonthReport(ReportRequest request);
}