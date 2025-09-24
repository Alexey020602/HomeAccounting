using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;

namespace Shared.Utils.BarCode;

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
                ]
            },
            AutoRotate = true,
        })
        .AddTransient<IBarcodeService, BarcodeService>();
}