using Authorization.Contracts;
using Fns.Contracts;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Receipts.Contracts;
using Receipts.Core.AddReceipt.BarCode;
using Shared.Utils.Model;
using Shared.Web;

namespace Checks.Api;

public class ReceiptsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChecks([FromQuery] GetChecks checksQuery) =>
        Ok(await mediator.Send(checksQuery));

    [HttpPut]
    public async Task<IActionResult> AddCheck([FromBody] CheckRequest checkRequest)
    {
        await mediator.Send(new AddCheckCommand()
        {
            ReceiptData = new(
                new ReceiptFiscalData(
                    checkRequest.Fn,
                    checkRequest.Fd,
                    checkRequest.Fp,
                    checkRequest.S,
                    checkRequest.T
                ),
                HttpContext.User.GetUserId()
            ),
        });
        return Ok();
    }

    [HttpPut("file")]
    public async Task<IActionResult> AddCheckWithFile(IFormFile file)
    {
        await mediator.Send(new AddImageCheckCommand()
        {
            ImageBytes = await file.OpenReadStream().ReadAsByteArrayAsync(),
            UserId = HttpContext.User.GetUserId(),
        });
        return Ok();
    }
}