using Refit;
using Reports.Contracts;
using Shared.Blazor.Attributes;
using ReportRequest = Reports.Contracts.ReportRequest;

namespace BlazorConsolidated.Api;

[ApiAuthorizable("reports/month")]
[Headers("Authorization: Bearer")]
public interface IReportsApi
{
    [Put("/")]
    Task<ReportDto> GetMonthReport(ReportRequest request);
}