using Refit;
using Reports.Contracts;
using Shared.Blazor.Attributes;
using ReportRequest = Reports.Contracts.ReportRequest;

namespace Reports.UI;

[ApiAuthorizable("reports/month")]
[Headers("Authorization: Bearer")]
public interface IReportsApi
{
    [Put("/")]
    Task<ReportDto> GetMonthReport(ReportRequest request);
}