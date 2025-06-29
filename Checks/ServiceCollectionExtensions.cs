using Checks.Contracts;
using Checks.Core;
using Checks.Core.BarCode;
using Checks.DataBase;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Handlers;
using SkiaSharp;
using ZXing;
using ZXing.SkiaSharp;

namespace Checks.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCheckModule(this IServiceCollection services/*, IConfiguration configuration*/)
    {
        return services.AddTransient<IBarcodeReader<SKBitmap>, BarcodeReader>()
                .AddScoped<ICheckRepository, CheckRepository>()
            .AddScoped<IReceiptSaveService, ReceiptSaveService>()
            .AddScoped<IReceiptService, ReceiptService>()
            .AddScoped<ICheckReceiptService, CheckReceiptService>()
            .AddScoped<ICheckUseCase, CheckUseCase>()
            .AddScoped<IHandleMessages<ReceiptCategorized>, ReceiptCategorizedHandler>()
            .AddScoped<IHandleMessages<ReceiptDataReceived>, ReceiptDataReceivedHandler>()
            .AddTransient<IBarcodeService, BarcodeService>()
            .Decorate<IBarcodeService, TelemetryBarcodeServiceDecorator>();
    }
}