using Mediator;
using Microsoft.AspNetCore.Mvc;
using Reports.Contracts;
using Reports.Core;
using Shared.Utils.Model;
using Shared.Web;

namespace Reports.Api;

public class ReportsController(IMediator mediator): ApiControllerBase
{
    [HttpGet("month")]
    public async Task<IActionResult> GetMonthReport([FromQuery] ReportRequest request) =>
        new JsonResult(await mediator.Send(new GetPeriodReportQuery(request.BudgetId, request.Range, HttpContext.User.GetUserId())));
}