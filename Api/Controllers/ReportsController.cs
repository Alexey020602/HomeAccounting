using Core.Model.Report;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportsController(IReportUseCase reportUseCase): ControllerBase
{
    [HttpGet("month")]
    public async Task<IActionResult> GetMonthReport([FromQuery] ReportRequest request) =>
        new JsonResult(await reportUseCase.GetReport(request));
}