using Checks.Api.BarCode;
using Microsoft.AspNetCore.Http;

namespace Checks.Api;

public static class BarcodeServiceExtension
{
    public static Task<string> ReadBarcodeAsync(this IBarcodeService service, IFormFile file)
    {
        return service.ReadBarcodeAsync(file.OpenReadStream());
    }
}