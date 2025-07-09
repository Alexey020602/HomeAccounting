using Budgets.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Budgets.Api;

public class BudgetsController(IAccountingSettingsService accountingSettingsService): ApiControllerBase
{
    [HttpGet("period")]
    public async Task<IActionResult> GetPeriod() => Ok(await accountingSettingsService.GetFirstDayOfAccountingPeriod());
}