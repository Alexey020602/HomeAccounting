using Checks.Contracts;
using Checks.Core;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;
using Wolverine;

namespace Checks.Api;

public class OtherChecksController(IMessageBus messageBus): ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChecks([FromQuery] GetChecksQuery checksQuery) => Ok(
        (await messageBus.InvokeAsync<ChecksResponse>(checksQuery)).Checks
        );
}