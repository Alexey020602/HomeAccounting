using Receipts.Core;
using Receipts.DataBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Handlers;
using Receipts.Contracts;
using Receipts.Core.AddReceipt;
using Receipts.Core.AddReceipt.BarCode;
using Receipts.Core.GetReceipts;
using Receipts.Core.ReceiptCategorization;
using Receipts.Core.ReceiptSaving;
using Shared.Infrastructure;
using SkiaSharp;
using ZXing;
using ZXing.SkiaSharp;

namespace Checks.Api;

public static class ReceiptsModule
{
    public static IHostApplicationBuilder AddReceiptsModule(this IHostApplicationBuilder builder, string databaseServiceName/*, IConfiguration configuration*/)
    {
        builder.AddDbContext<ReceiptsContext>(
            databaseServiceName,
            optionsAction: options => options.SetupChecksForDevelopment(),
            npgsqlOptionsAction: options => options.MigrationsHistoryTable(DbConstants.MigrationTableName, ReceiptsDbConstants.ShemaName)
            );
        builder.Services.AddTransient<IBarcodeReader<SKBitmap>, BarcodeReader>()
                .AddScoped<IGetReceiptsService, GetReceiptsService>()
            .AddScoped<IReceiptSaveService, ReceiptSaveService>()
            .AddScoped<IReceiptService, ReceiptService>()
            .AddScoped<ICheckReceiptService, CheckReceiptService>()
            .AddScoped<IGetProductsService, GetProductsService>()
            .AddScoped<IHandleMessages<ReceiptCategorized>, ReceiptCategorizedHandler>()
            .AddScoped<IHandleMessages<ReceiptDataReceived>, ReceiptDataReceivedHandler>()
            .AddTransient<IBarcodeService, BarcodeService>()
            .Decorate<IBarcodeService, TelemetryBarcodeServiceDecorator>();
        return builder;
    }
}