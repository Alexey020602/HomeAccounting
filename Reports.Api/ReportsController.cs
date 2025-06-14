using Microsoft.AspNetCore.Mvc;
using Reports.Contracts;
using Reports.Core;
using Shared.Web;

namespace Reports.Api;

public class ReportsController(IReportUseCase reportUseCase): ApiControllerBase
{
    [HttpPut("month")]
    public async Task<IActionResult> GetMonthReport(ReportRequest request) =>
        new JsonResult(await reportUseCase.GetReport(request));
}