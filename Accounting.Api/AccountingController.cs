using Accounting.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Accounting.Api;

public class AccountingController(IAccountingSettingsService accountingSettingsService): ApiControllerBase
{
    [HttpGet("period")]
    public async Task<IActionResult> GetPeriod() => Ok(await accountingSettingsService.GetFirstDayOfAccountingPeriod());
}