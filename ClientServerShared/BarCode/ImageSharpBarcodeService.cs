using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
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
        using var rgba = await Image.LoadAsync<Rgba32>(stream);
        rgba.Mutate(x => x.AutoOrient());
        using var baseL8 = rgba.CloneAs<L8>();

        foreach (var variant in BuildVariants(baseL8))
        {
            using (variant)
            {
                var result = reader.Decode(variant);
                if (result?.Text is { Length: > 0 } text)
                    return text;
            }
        }

        throw new BarcodeException("Cannot decode image");
    }

    static IEnumerable<Image<L8>> BuildVariants(Image<L8> image)
    {
        using var src = CroppedImages(image);
        // 1) как есть
        yield return src.Clone();

        // 2) апскейл если мелко
        yield return DownscaleImage(src);

        // 3) blur+contrast
        yield return src.Clone(x => { x.GaussianBlur(0.8f); x.Contrast(1.25f); });

        // 4) blur посильнее + порог
        yield return src.Clone(x => { x.GaussianBlur(1.2f); x.BinaryThreshold(0.5f); });

        // 5) инверсия (на случай бликов/темного UI)
        yield return src.Clone(x => x.Invert());

        // 6) инверсия + blur + порог
        yield return src.Clone(x => { x.Invert(); x.GaussianBlur(0.8f); x.BinaryThreshold(0.5f); });

        // 7–9) пороги 0.45 / 0.55
        yield return src.Clone(x => x.BinaryThreshold(0.45f));
        yield return src.Clone(x => x.BinaryThreshold(0.55f));
    }

    static Image<L8> CroppedImages(Image<L8> src, float factor = 0.6f)
    {
        var width = (int) (src.Width * factor);
        var height = (int) (src.Height * 0.5);
        var x = (src.Width - width) / 2;
        var y = (src.Height) / 2;

        return src.Clone(c => c.Crop(new Rectangle(x, y, width, height)));
    }
    private static Image<L8> DownscaleImage(Image<L8> image, int maxSize = 800)
    {
        return image.Clone(ctx =>
            {
                var width = image.Width;
                var height = image.Height;
                
                var side = Math.Min(width, height);

                if (side > maxSize)
                {
                    var scale = (double) maxSize / side;

                    ctx.Resize(new ResizeOptions
                    {
                        Size = new Size(
                            (int)Math.Round(width * scale),
                            (int)Math.Round(height * scale)
                        ),
                        Mode = ResizeMode.Max,
                        Sampler = KnownResamplers.Bicubic,
                    });

                }
                
                ctx.GaussianBlur(0.8f);

                // Подкрутить контраст/яркость (подберите)
                ctx.Contrast(1.25f);
                ctx.Brightness(1.05f);
            }
        );
    }
    
    /// <summary>
    /// Reads barcode from byte array using ImageSharp.
    /// </summary>
    public ValueTask<string> ReadBarcodeAsync(byte[] imageBytes)
    {
        using var image = Image.Load<L8>(imageBytes);
            var result = reader.Decode(image) ?? throw new BarcodeException("Cannot decode image");
            var resultText = result.Text ?? throw new BarcodeException("Result not contains text", result.ResultMetadata);
            return ValueTask.FromResult(resultText);
    }
}
