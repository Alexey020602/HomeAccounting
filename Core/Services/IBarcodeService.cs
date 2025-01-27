namespace Core.Services;

public interface IBarcodeService
{
    public Task<string> ReadBarcodeAsync(Stream stream);
}