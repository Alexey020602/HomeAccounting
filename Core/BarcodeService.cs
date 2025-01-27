using Core.Extensions;
using Core.Services;
using SkiaSharp;
using ZXing;

namespace Core;

public class BarcodeService(IBarcodeReader<SKBitmap> reader): IBarcodeService
{
    public async Task<string> ReadBarcodeAsync(Stream stream)
    {
        var imageBytes = await stream.ReadAsByteArrayAsync();
        var barcodeImage = SKImage.FromEncodedData(imageBytes);
        var bitmap = SKBitmap.FromImage(barcodeImage);
        var result = reader.Decode(bitmap);
        return result.Text;
    }
}