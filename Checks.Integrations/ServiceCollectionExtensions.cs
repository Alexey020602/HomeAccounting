using Api;
using Checks.Core;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using ZXing;
using ZXing.SkiaSharp;

namespace Checks.Integrations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCheckModule(this IServiceCollection services/*, IConfiguration configuration*/)
    {
        services.AddTransient<IBarcodeReader<SKBitmap>, BarcodeReader>();
        services.AddScoped<ICheckUseCase, CheckUseCase>();
        services.AddTransient<IBarcodeService, BarcodeService>();
        services.AddScoped<ICheckSource, FnsCheckSource>();
        return services;
    }
}