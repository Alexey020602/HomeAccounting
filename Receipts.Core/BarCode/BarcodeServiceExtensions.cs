namespace Receipts.Core.BarCode;

public static class BarcodeServiceExtensions
{
    public static async Task<string> ReadBarcodeAsync(
        this IBarcodeService barcodeService,
        Stream stream)
    {
        return await barcodeService.ReadBarcodeAsync(await stream.ReadAsByteArrayAsync());
    }
}