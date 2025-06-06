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
        services.AddTransient<IBarcodeReader<SKBitmap>, BarcodeReader>();
        services.AddScoped<ICheckUseCase, CheckUseCase>();
        services.AddTransient<IBarcodeService, BarcodeService>();
        // services.AddScoped<ICheckSource, FnsCheckSource>();
        return services;
    }
}