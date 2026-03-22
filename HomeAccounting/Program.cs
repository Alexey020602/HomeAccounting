using BlazorConsolidated;
using HomeAccounting;
using HomeAccounting.Budgets;
using HomeAccounting.Budgets.Data.Database.Seeding;
using HomeAccounting.Categories;
using HomeAccounting.Categories.Data.DataBase.Seeding;
using HomeAccounting.Common.Infrastructure.Events.EventBus;
using HomeAccounting.ReceiptProcessing;
using HomeAccounting.Users;
using HomeAccounting.Users.Data.Database;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Display;
using Serilog.Sinks.OpenTelemetry;
using Serilog.Sinks.SystemConsole.Themes;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();


builder.AddServiceDefaults();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<UsersContext>("postgres", tags: ["ready"]);

builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();

builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerAuthenticationSchemeTransformer>());
builder.Services.AddSerilog((configuration) =>
{
    configuration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)

        .WriteTo.Console(
            theme: ConsoleTheme.None,
            applyThemeToRedirectedOutput: false,
            outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        )

        .WriteTo.OpenTelemetry(includedData:
            IncludedData.MessageTemplateTextAttribute |
            IncludedData.SpanIdField |
            IncludedData.TraceIdField)

        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", "HomeAccounting");
});

builder.Services.AddProblemDetails();

builder.Services.AddTransient<HttpLoggingHandler>();
builder.Services.AddEventBus();

var databaseServiceName = "homeaccounting-db";
builder.AddUsers(databaseServiceName);
builder.AddBudgets(databaseServiceName);
builder.AddCategories(databaseServiceName);
builder.Services.AddReceiptProcessingModule();

var app = builder.Build();

await app.MigrateUsersAsync();
await app.MigrateBudgetsAsync();
await app.MigrateCategoriesAsync();

app.UseCors(policyBuilder => policyBuilder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);
app.UseExceptionHandler();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.PersistentAuthentication = true;
    });
}


app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "Handled {RequestMethod} {RequestPath} {StatusCode} {Elapsed}";
    options.GetLevel = HomeAccounting.SerilogApplicationBuilderExtensions.DefaultGetLevel;
});

if(app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

var apiGroup = app.MapGroup("api").RequireAuthorization();

apiGroup.MapUsersEndpoints();
apiGroup.MapBudgetsEndpoints();
apiGroup.MapCategoriesEndpoints();

var productsGroup = app.MapGroup("api/products").AllowAnonymous();
productsGroup.MapProductsEndpoints();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Routes).Assembly)
    .AllowAnonymous();

await app.RunAsync();

