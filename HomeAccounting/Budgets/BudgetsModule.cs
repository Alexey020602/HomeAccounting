using ClientServerShared.BarCode;
using HomeAccounting.Budgets.Configuration;
using HomeAccounting.Budgets.Services;
using HomeAccounting.Budgets.Workers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Common;

namespace HomeAccounting.Budgets;

internal static class BudgetsModule
{
    public static void AddBudgets(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder.AddDatabase(databaseServiceName);

        builder.Services.AddScoped<IAuthorizationHandler, BudgetRequirementsAuthorizationHandler>();

        builder.Services
            .AddOptions<ReceiptProcessingOptions>()
            .BindConfiguration(ReceiptProcessingOptions.SectionName)
            .ValidateOnStart();

        builder.Services.AddSingleton<IValidateOptions<ReceiptProcessingOptions>, ReceiptProcessingOptionsValidator>();

        builder.Services.AddScoped<IReceiptProcessingOrchestrator, ReceiptProcessingOrchestrator>();
        builder.Services.AddHostedService<ReceiptRetryWorker>();
        
        builder.Services.AddImageSharpBarcode()
            .Decorate<IBarcodeService, TelemetryBarcodeServiceDecorator>();
    }
    
}