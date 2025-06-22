namespace Checks.Api.BarCode;

public interface IBarcodeService
{
    public Task<string> ReadBarcodeAsync(Stream stream);
}