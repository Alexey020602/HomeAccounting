using Accounting.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api;

[ApiController]
[Route("[controller]")]
public class AccountingController(IAccountingSettingsService accountingSettingsService): ControllerBase
{
    [HttpGet("period")]
    public async Task<IActionResult> GetPeriod() => Ok(await accountingSettingsService.GetFirstDayOfAccountingPeriod());
}