namespace Checks.Api;

public interface IBarcodeService
{
    public Task<string> ReadBarcodeAsync(Stream stream);
}