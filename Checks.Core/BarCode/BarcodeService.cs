using SkiaSharp;
using ZXing;

namespace Checks.Core.BarCode;

public class BarcodeService(IBarcodeReader<SKBitmap> reader) : IBarcodeService
{
    public Task<string> ReadBarcodeAsync(byte[] imageBytes)
    {
        var barcodeImage = SKImage.FromEncodedData(imageBytes) ??
                           throw new BarcodeException("Cannot create image from request");
        var bitmap = SKBitmap.FromImage(barcodeImage) ?? throw new BarcodeException("Cannot create Bitmap from image");
        var result = reader.Decode(bitmap) ?? throw new BarcodeException("Cannot decode image");
        return Task.FromResult(result.Text) ?? throw new BarcodeException("Result not contains text");
    }
}

public sealed class BarcodeException(string message) : Exception(message);