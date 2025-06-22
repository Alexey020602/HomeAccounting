using SkiaSharp;
using ZXing;

namespace Checks.Api.BarCode;

public class BarcodeService(IBarcodeReader<SKBitmap> reader) : IBarcodeService
{
    public async Task<string> ReadBarcodeAsync(Stream stream)
    {
        var imageBytes = await stream.ReadAsByteArrayAsync();
        var barcodeImage = SKImage.FromEncodedData(imageBytes) ??
                           throw new BarcodeException("Cannot create image from request");
        var bitmap = SKBitmap.FromImage(barcodeImage) ?? throw new BarcodeException("Cannot create Bitmap from image");
        var result = reader.Decode(bitmap) ?? throw new BarcodeException("Cannot decode image");
        return result.Text ?? throw new BarcodeException("Result not contains text");
    }
}

public sealed class BarcodeException(string message) : Exception(message);