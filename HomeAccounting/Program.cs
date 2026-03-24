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
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();


builder.AddServiceDefaults();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<UsersContext>("postgres", tags: ["ready"]);

builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();

builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerAuthenticationSchemeTransformer>());
builder.Services.AddSerilog(configuration =>
{
    configuration
        .AddDefaultSerilog()
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

