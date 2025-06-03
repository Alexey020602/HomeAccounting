using BlazorShared.Api.Attributes;
using Core.Reports.Model;
using Refit;
using Reports.Core;
using Reports.Dto;

namespace BlazorShared.Api;

[ApiAuthorizable("reports/month")]
[Headers("Authorization: Bearer")]
public interface IReportsApi
{
    [Put("/")]
    Task<Report> GetMonthReport(ReportRequest request);
}