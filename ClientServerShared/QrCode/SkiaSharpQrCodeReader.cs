using SkiaSharp;
using ZXing;

namespace ClientServerShared.QrCode;

public class SkiaSharpQrCodeReader(IBarcodeReader<SKBitmap> reader) : IQrCodeReader
{
    public async ValueTask<string> ReadQrCodeAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        var barcodeData = SKData.Create(memoryStream)??
                          throw new QrCodeReaderException("Cannot create image from request");

        var skBitmap = SKBitmap.Decode(barcodeData) ?? throw new QrCodeReaderException("Cannot create Bitmap from data");
        var result = reader.Decode(skBitmap) ?? throw new QrCodeReaderException("Cannot decode image"); 
        var resultText = result.Text ?? throw new QrCodeReaderException("Result not contains text", result.ResultMetadata);
        return resultText;
    }
}

public sealed class QrCodeReaderException(string message, IDictionary<ResultMetadataType, object>? resultMetadata = null) : Exception(message)
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