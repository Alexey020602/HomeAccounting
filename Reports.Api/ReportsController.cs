using Authorization.Contracts;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Reports.Contracts;
using Reports.Core;
using Shared.Model;
using Shared.Web;

namespace Reports.Api;

public class ReportsController(IMediator mediator): ApiControllerBase
{
    [HttpPut("month")]
    public async Task<IActionResult> GetMonthReport(ReportRequest request) =>
        new JsonResult(await mediator.Send(new GetPeriodReportQuery(request.Range, HttpContext.User.GetLogin())));
}