using Authorization.Contracts;
using Fns.Contracts;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Receipts.Contracts;
using Receipts.Core.AddReceipt.BarCode;
using Shared.Model.Requests;
using Shared.Web;

namespace Checks.Api;

public class ReceiptsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChecks([FromQuery] GetChecks checksQuery) => Ok(await mediator.Send((GetChecks) checksQuery));

    [HttpPut]
    public async Task<IActionResult> AddCheck([FromBody] CheckRequest checkRequest)
    {
        await mediator.Send(new AddCheckCommand()
        {
            Login = HttpContext.User.GetLogin(),
            FiscalData = new ReceiptFiscalData(checkRequest.Fd, checkRequest.Fn, checkRequest.Fp, checkRequest.S,
                checkRequest.T),
        });
        return Ok();
    }

    [HttpPut("file")]
    public async Task<IActionResult> AddCheckWithFile(IFormFile file)
    {

        await mediator.Send(new AddImageCheckCommand()
        {
            ImageBytes = await file.OpenReadStream().ReadAsByteArrayAsync(),
            Login = HttpContext.User.GetLogin(),
        });
        return Ok();
    }
}