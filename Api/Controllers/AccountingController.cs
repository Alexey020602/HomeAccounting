using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountingController(IAccountingSettingsService accountingSettingsService): ControllerBase
{
    [HttpGet("period")]
    public async Task<IActionResult> GetPeriod() => Ok(await accountingSettingsService.GetFirstDayOfAccountingPeriod());
}