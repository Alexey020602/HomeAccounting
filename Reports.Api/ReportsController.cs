using Core.Reports.Model;
using Microsoft.AspNetCore.Mvc;
using Reports.Core;

namespace Reports.Api;

[ApiController]
[Route("[controller]")]
public class ReportsController(IReportUseCase reportUseCase): ControllerBase
{
    [HttpPut("month")]
    public async Task<IActionResult> GetMonthReport(ReportRequest request) =>
        new JsonResult(await reportUseCase.GetReport(request));
}