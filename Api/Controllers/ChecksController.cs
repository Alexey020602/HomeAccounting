using Authorization.Extensions;
using Core.Model.Requests;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChecksController(ICheckUseCase checkUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChecks() => Ok(await checkUseCase.GetChecksAsync());

    //todo подумать над изменением типа запроса на Put
    [HttpPost]
    public async Task<IActionResult> AddCheck([FromBody] CheckRequest checkRequest) =>
        Ok(await checkUseCase.SaveCheck(checkRequest, HttpContext.User.CreateUser()));

    [HttpPost("file")]
    public async Task<IActionResult> AddCheckWithFile(IFormFile file, DateTimeOffset addedDate,
        [FromServices] IBarcodeService barcodeService)
    {
        var qrResult = await barcodeService.ReadBarcodeAsync(file.OpenReadStream());
        return Ok(await checkUseCase.SaveCheck(
            new RawCheckRequest
            {
                QrRaw = qrResult,
                AddedTime = addedDate,
            },
            HttpContext.User.CreateUser()));
    }
}

public record FileCheckRequest(IFormFile File, DateTimeOffset AddedDate);