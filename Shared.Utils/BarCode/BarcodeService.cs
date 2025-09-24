using SkiaSharp;
using ZXing;

namespace Shared.Utils.BarCode;

public class BarcodeService(IBarcodeReader<SKBitmap> reader) : IBarcodeService
{
    public async ValueTask<string> ReadBarcodeAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return ReadBarcodeFromData(SKData.Create(memoryStream)??
                                    throw new BarcodeException("Cannot create image from request"));
    }
    public ValueTask<string> ReadBarcodeAsync(byte[] imageBytes)
    {
        var barcodeImage = SKImage.FromEncodedData(imageBytes) ??
                           throw new BarcodeException("Cannot create image from request");
        var resultText = ReadBarcodeFromImage(barcodeImage);
        return ValueTask.FromResult<string>(resultText);
    }

    private string ReadBarcodeFromImage(SKImage barcodeImage)
    {
        var bitmap = SKBitmap.FromImage(barcodeImage) ?? throw new BarcodeException("Cannot create Bitmap from image");
        var resultText = ReadFromBitmap(bitmap);
        return resultText;
    }
    
    private string ReadBarcodeFromData(SKData barcodeData) => 
        ReadFromBitmap(SKBitmap.Decode(barcodeData) ?? throw new BarcodeException("Cannot create Bitmap from data"));

    private string ReadFromBitmap(SKBitmap bitmap)
    {
        var result = reader.Decode(bitmap) ?? throw new BarcodeException("Cannot decode image"); 
        Console.WriteLine(result);
        var resultText = result.Text ?? throw new BarcodeException("Result not contains text", result.ResultMetadata);
        return resultText;
    }
}

public sealed class BarcodeException(string message, IDictionary<ResultMetadataType, object>? resultMetadata = null) : Exception(message)
{
    private readonly IDictionary<ResultMetadataType, object> resultMetadata = resultMetadata ?? new Dictionary<ResultMetadataType, object>();
    public override string ToString()
    {
        return MetadataDescription + base.ToString();
    }

    private string MetadataDescription => resultMetadata.Any() 
        ? string.Join("\n", resultMetadata.Select(pair => $"{pair.Key}: {pair.Value}")) 
        : string.Empty;
}