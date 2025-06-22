using Authorization.Contracts;
using Checks.Api.BarCode;
using Checks.Api.Requests;
using Checks.Contracts;
using Checks.Core;
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

    //todo подумать над изменением типа запроса на Put
    [HttpPost]
    public async Task<IActionResult> AddCheck([FromBody] CheckRequest checkRequest) =>
        Ok(await checkUseCase.SaveCheck(checkRequest.SaveCheckRequest(HttpContext.User.GetLogin())));

    [HttpPost("file")]
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