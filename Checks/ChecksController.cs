using Authorization.Contracts;
using Checks.Api.Requests;
using Checks.Contracts;
using Checks.Core;
using Fns;
using Fns.Contracts;
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
        var storeCheck = await messageBus.InvokeAsync<StoreCheckCommand>(checkRequest);
        return Ok(storeCheck);
    }

    [HttpPost("file")]
    public async Task<IActionResult> AddCheckWithFile(IFormFile file, DateTimeOffset addedDate,
        [FromServices] IBarcodeService barcodeService)
    {  
        var login = HttpContext.User.GetLogin();
        var qrResult = await barcodeService.ReadBarcodeAsync(file.OpenReadStream());
        var request = new CheckRequest(qrResult, addedDate);
        var checkRequest = new SaveCheckCommand()
        {
            Login = login,
            Fn = request.Fn, 
            Fd = request.Fd, 
            Fp = request.Fp, 
            S = request.S, 
            T = request.T
        };
        try
        {
            var storeCheck = await messageBus.InvokeAsync<LoadCheckCommand>(checkRequest);
            if (storeCheck == null) return Conflict();
            return Ok(storeCheck);
        }
        catch (TestException ex)
        {
            return BadRequest(ex.Message);
        }

        // await messageBus.PublishAsync(storeCheck);
    }
}