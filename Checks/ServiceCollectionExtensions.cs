using Checks.Api.BarCode;
using Checks.Contracts;
using Checks.Core;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using ZXing;
using ZXing.SkiaSharp;

namespace Checks.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCheckModule(this IServiceCollection services/*, IConfiguration configuration*/)
    {
        return services.AddTransient<IBarcodeReader<SKBitmap>, BarcodeReader>()
            .AddScoped<ICheckUseCase, CheckUseCase>()
            .AddTransient<IBarcodeService, BarcodeService>()
            .Decorate<IBarcodeService, TelemetryBarcodeServiceDecorator>();
    }
}