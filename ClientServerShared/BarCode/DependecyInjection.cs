using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.ImageSharp;
using ZXing.SkiaSharp;

namespace ClientServerShared.BarCode;

public static class DependecyInjection
{
    public static IServiceCollection AddBarcode(this IServiceCollection services) => services
        .AddTransient<IBarcodeReader<SKBitmap>>((_) => new BarcodeReader()
        {
            Options = new DecodingOptions()
            {
                // TryHarder = true,
                PossibleFormats = [
                    BarcodeFormat.QR_CODE,
                ],
                TryHarder = true,
                TryInverted = true,
            },
            
            AutoRotate = true,
        })
        
        .AddTransient<IBarcodeService, SkiaSharpBarcodeService>();
    
    /// <summary>
    /// Adds ImageSharp-based barcode service as an alternative to SkiaSharp-based service.
    /// </summary>
    public static IServiceCollection AddImageSharpBarcode(this IServiceCollection services) => services
        .AddTransient<IBarcodeReader<Image<L8>>>(_ => new ZXing.ImageSharp.BarcodeReader<L8>()
        {
            Options = new DecodingOptions()
            {
                PossibleFormats = [
                    BarcodeFormat.QR_CODE,
                ],
                TryHarder = true,
                TryInverted = true,
            },
            AutoRotate = true,
        })
        .AddTransient<IBarcodeService, ImageSharpBarcodeService>();
}