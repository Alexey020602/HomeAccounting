using Authorization.Contracts;
using Checks.Api.Requests;
using Checks.Contracts;
using Checks.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Model;
using Shared.Model.Requests;
using Shared.Web;
using Wolverine;

namespace Checks.Api;
public class ChecksController(ICheckUseCase checkUseCase, IMessageBus messageBus) : ApiControllerBase
{
    // [HttpGet]
    // public async Task<IActionResult> GetChecks([FromQuery] GetChecksQuery checksQuery) => Ok(await checkUseCase.GetChecksAsync(checksQuery));
    
    [HttpGet]
    public async Task<IActionResult> GetChecks([FromQuery] GetChecksQuery checksQuery) => Ok(
        (await messageBus.InvokeAsync<ChecksResponse>(checksQuery)).Checks
    );
    //todo подумать над изменением типа запроса на Put
    [HttpPost]
    public async Task<IActionResult> AddCheck([FromBody] SaveCheckRequest request)
    {
        var login = HttpContext.User.GetLogin();
        var checkRequest = new SaveCheckCommand()
        {
            Login = login,
            Fn = request.Fn,
            Fd = request.Fd,
            Fp = request.Fp,
            S = request.S,
            T = request.T
        };
        await messageBus.InvokeAsync(checkRequest);
        // await checkUseCase.SaveCheck(checkRequest.SaveCheckRequest(HttpContext.User.GetLogin()))
        return Ok();
    }

    [HttpPost("file")]
    public async Task<IActionResult> AddCheckWithFile(IFormFile file, DateTimeOffset addedDate,
        [FromServices] IBarcodeService barcodeService)
    {   
        var login = HttpContext.User.GetLogin();
        var qrResult = await barcodeService.ReadBarcodeAsync(file.OpenReadStream());
        var request = new CheckRequest(qrResult, addedDate);
        await messageBus.InvokeAsync(new SaveCheckCommand()
        {
            Login = login,
            Fn = request.Fn, 
            Fd = request.Fd, 
            Fp = request.Fp, 
            S = request.S, 
            T = request.T
        });
        
        return Ok();
        // return Ok(await checkUseCase.SaveCheck(
        //     new CheckRequest(qrResult, addedDate)
        //         .SaveCheckRequest(HttpContext.User.GetLogin())
        // ));
    }
}