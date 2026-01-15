using BlazorConsolidated;
using MudBlazor.Extensions;
using MyBudgets;
using MyBudgets.Budgets;
using MyBudgets.Budgets.Data.Database;
using MyBudgets.Common.Http;
using MyBudgets.Users;
using MyBudgets.Users.CheckLogin;
using MyBudgets.Users.Data.Database;
using MyBudgets.Users.GetUser;
using MyBudgets.Users.Login;
using MyBudgets.Users.Register;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;
using ServiceDefaults;
using HttpLoggingHandler = ServiceDefaults.HttpLoggingHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();


builder.AddServiceDefaults();

builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();

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

builder.Services.AddProblemDetails();

builder.Services.AddTransient<HttpLoggingHandler>();


var databaseServiceName = "HomeAccounting";
builder.AddUsers(databaseServiceName);
builder.AddBudgets(databaseServiceName);

var app = builder.Build();

await app.MigrateUsersAsync();
await app.MigrateBudgetsAsync();
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
    app.MapScalarApiReference();
}


app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "Handled {RequestMethod} {RequestPath} {StatusCode} {Elapsed}";
    options.GetLevel = MyBudgets.SerilogApplicationBuilderExtensions.DefaultGetLevel;
});

app.UseHttpsRedirection();

var apiGroup = app.MapGroup("api").RequireAuthorization();

apiGroup.MapUsersEndpoints();
apiGroup.MapBudgetsEndpoints();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Routes).Assembly)
    .AllowAnonymous();

await app.RunAsync();

