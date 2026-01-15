using Budgets.Api;
using Api;
using Authorization;
using Authorization.Core.Login;
using Authorization.DataBase;
using Authorization.DependencyInjection;
using Authorization.UI.Pages;
// using BlazorConsolidated;
using Budgets.Core.GetBudgets;
using Budgets.DataBase;
using Checks.Api;
using Fns;
using Mediator;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using Receipts.Contracts;
using Receipts.Core.GetReceipts;
using Receipts.Core.ReceiptSaving;
using Receipts.DataBase;
using Reports.Api;
using Reports.Core;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;
using ServiceDefaults;
using Shared.Utils;
using SerilogApplicationBuilderExtensions = Api.SerilogApplicationBuilderExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.AddServiceDefaults();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerAuthenticationSchemeTransformer>());
builder.Services.AddSerilog((configuration) =>
{
    configuration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .WriteTo.Console()
        .WriteTo.OpenTelemetry(includedData: IncludedData.MessageTemplateTextAttribute | IncludedData.SpanIdField |
                                             IncludedData.TraceIdField)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", "HomeAccounting");
});
builder.Services.AddTransient<HttpLoggingHandler>();


builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
const string databaseName = "HomeAccounting";
builder.AddReceiptsModule(databaseName);
builder.Services.AddFnsModule();
builder.AddBudgetsModule(databaseName);
builder.Services.AddReportsModule();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.AddAuthorization(databaseName);

builder.Services.Configure<RouteOptions>(options => { options.SuppressCheckForUnhandledSecurityMetadata = true; });

builder.Services.AddMediator((MediatorOptions options) =>
{
    options.Assemblies =
    [
        typeof(GetChecks).Assembly,
        typeof(GetChecksHandler).Assembly,
        typeof(GetReportHandler).Assembly,
        // typeof(GetBudgetsHandler).Assembly,
        typeof(LoginHandler).Assembly,
    ];
    options.PipelineBehaviors = [typeof(TelemetryPipelineBehaviour<,>)];
    options.ServiceLifetime = ServiceLifetime.Scoped;
});
builder.Services.AddRebus(configure =>
    {
        const string queueName = "HomeAccounting";
        return configure
            .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), queueName))
            .Logging(logging => logging.Serilog())
            .Options(o => o.EnableDiagnosticSources())
            .Routing(cofigurer => cofigurer
                .TypeBased()
                .MapAssemblyOf<ReceiptCategorized>(queueName));
    }
);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var applicationContext = scope.ServiceProvider.GetRequiredService<ReceiptsContext>();
    await applicationContext.Database.MigrateAsync();
    var authorizationContext = scope.ServiceProvider.GetRequiredService<AuthorizationContext>();
    await authorizationContext.Database.MigrateAsync();
    var budgetContext = scope.ServiceProvider.GetRequiredService<BudgetsContext>();
    await budgetContext.Database.MigrateAsync();
}

app.UseCors(policyBuilder => policyBuilder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);
app.UseExceptionHandler();
app.UseStatusCodePages();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "openapi/v1.json";
        options.SwaggerRoutes.Add(new SwaggerUiRoute("Api", "/openapi/v1.json"));
    });
}


app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "Handled {RequestMethod} {RequestPath} {StatusCode} {Elapsed}";
    options.GetLevel = SerilogApplicationBuilderExtensions.DefaultGetLevel;
});

app.MapControllers()
    .RequireAuthorization();

var apiGroup = app
    .MapGroup("/api");
    apiGroup
        .MapAuthorization();
apiGroup
    .MapBudgets();

apiGroup.MapBudgetGroup().MapReceiptEndpoints();
app.MapStaticAssets();
// app.MapRazorComponents<App>()
//     .AddInteractiveWebAssemblyRenderMode()
//     .AddAdditionalAssemblies(
//         typeof(Login).Assembly,
//         typeof(Receipts.UI.Receipts).Assembly,
//         typeof(MonthReportComponent).Assembly,
//         typeof(BudgetsPage).Assembly,
//         typeof(Routes).Assembly
//     )
//     .AllowAnonymous();

app.Run();

namespace Api
{
}