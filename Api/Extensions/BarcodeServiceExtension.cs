using Core.Services;

namespace Api.Extensions;

public static class BarcodeServiceExtension
{
    public static Task<string> ReadBarcodeAsync(this IBarcodeService service, IFormFile file) =>
        service.ReadBarcodeAsync(file.OpenReadStream());
}