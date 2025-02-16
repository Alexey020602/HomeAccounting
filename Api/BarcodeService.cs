using Core.Extensions;
using Core.Services;
using SkiaSharp;
using ZXing;

namespace Api;

public class BarcodeService(IBarcodeReader<SKBitmap> reader) : IBarcodeService
{
    public async Task<string> ReadBarcodeAsync(Stream stream)
    {
        var imageBytes = await stream.ReadAsByteArrayAsync();
        var barcodeImage = SKImage.FromEncodedData(imageBytes) ??
                           throw new Exception("Cannot create image from request");
        var bitmap = SKBitmap.FromImage(barcodeImage) ?? throw new Exception("Cannot create Bitmap from image");
        var result = reader.Decode(bitmap) ?? throw new Exception("Cannot decode image");
        return result.Text ?? throw new Exception("Result not contains text");
    }
}