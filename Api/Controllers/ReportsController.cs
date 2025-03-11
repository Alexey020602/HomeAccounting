using Core.Model.Report;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportsController(IReportUseCase reportUseCase): ControllerBase
{
    [HttpPut("month")]
    public async Task<IActionResult> GetMonthReport(ReportRequest request) =>
        new JsonResult(await reportUseCase.GetReport(request));
}