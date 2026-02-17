using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.ImageSharp;

namespace ClientServerShared.BarCode;

/// <summary>
/// Service for reading barcodes from images using ImageSharp library.
/// </summary>
public class ImageSharpBarcodeService(IBarcodeReader<Image<L8>> reader) : IBarcodeService
{
    /// <summary>
    /// Reads barcode from stream using ImageSharp.
    /// </summary>
    public async ValueTask<string> ReadBarcodeAsync(Stream stream)
    {
        var image = await Image.LoadAsync<L8>(stream);
        try
        {
            var result = reader.Decode(image) ?? throw new BarcodeException("Cannot decode image");
            var resultText = result.Text ?? throw new BarcodeException("Result not contains text", result.ResultMetadata);
            return resultText;
        }
        finally
        {
            image.Dispose();
        }
    }

    /// <summary>
    /// Reads barcode from byte array using ImageSharp.
    /// </summary>
    public ValueTask<string> ReadBarcodeAsync(byte[] imageBytes)
    {
        var image = Image.Load<L8>(imageBytes);
        try
        {
            var result = reader.Decode(image) ?? throw new BarcodeException("Cannot decode image");
            var resultText = result.Text ?? throw new BarcodeException("Result not contains text", result.ResultMetadata);
            return ValueTask.FromResult(resultText);
        }
        finally
        {
            image.Dispose();
        }
    }
}
