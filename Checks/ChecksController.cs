using Authorization.Contracts;
using Checks.Api.Requests;
using Checks.Contracts;
using Checks.Core;
using Checks.Core.BarCode;
using Fns.Contracts;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Model;
using Shared.Model.Requests;
using Shared.Web;

namespace Checks.Api;
public class ChecksController(ICheckUseCase checkUseCase) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChecks([FromQuery] GetChecksQuery checksQuery) => Ok(await checkUseCase.GetChecksAsync(checksQuery));

    [HttpPut]
    public async Task<IActionResult> AddCheck([FromBody] CheckRequest checkRequest) =>
        Ok(await checkUseCase.SaveCheck(checkRequest.SaveCheckRequest(HttpContext.User.GetLogin())));

    [HttpPut("file")]
    public async Task<IActionResult> AddCheckWithFile(IFormFile file, DateTimeOffset addedDate,
        [FromServices] IBarcodeService barcodeService)
    {
        var qrResult = await barcodeService.ReadBarcodeAsync(file.OpenReadStream());
        return Ok(await checkUseCase.SaveCheck(
            new CheckRequest(qrResult, addedDate)
                .SaveCheckRequest(HttpContext.User.GetLogin())
        ));
    }
}

public class OtherChecksController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChecks([FromQuery] GetChecksQuery checksQuery) => Ok(await mediator.Send((GetChecks) checksQuery));

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
    public async Task<IActionResult> AddCheckWithFile(IFormFile file, DateTimeOffset addedDate/*,
        [FromServices] IBarcodeService barcodeService*/)
    {

        await mediator.Send(new AddImageCheckCommand()
        {
            ImageBytes = await file.OpenReadStream().ReadAsByteArrayAsync(),
            Login = HttpContext.User.GetLogin(),
        });
        return Ok();
        // var qrResult = await barcodeService.ReadBarcodeAsync(file.OpenReadStream());
        // return Ok(await checkUseCase.SaveCheck(
        //     new CheckRequest(qrResult, addedDate)
        //         .SaveCheckRequest(HttpContext.User.GetLogin())
        // ));
    }
}