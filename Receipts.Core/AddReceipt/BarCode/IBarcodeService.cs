namespace Receipts.Core.AddReceipt.BarCode;

public interface IBarcodeService
{
    public Task<string> ReadBarcodeAsync(byte[] imageBytes);
}