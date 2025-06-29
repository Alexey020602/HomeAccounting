namespace Checks.Core.BarCode;

public interface IBarcodeService
{
    public Task<string> ReadBarcodeAsync(byte[] imageBytes);
}