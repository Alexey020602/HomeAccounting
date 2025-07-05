using Accounting.Api;
using Api;
using Authorization.Core.Login;
using Authorization.DataBase;
using Authorization.DependencyInjection;
using BlazorShared.Layouts;
using Checks.Api;
using Fns;
using Mediator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NSwag.AspNetCore;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using Receipts.Contracts;
using Receipts.Core;
using Receipts.Core.AddReceipt;
using Receipts.Core.ReceiptSaving;
using Receipts.DataBase;
using Reports.Api;
using Reports.Contracts;
using Reports.Core;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;
using ServiceDefaults;
using Shared.Infrastructure;
using Shared.Utils;
using WebClient.Components;
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
        .WriteTo.OpenTelemetry(includedData: IncludedData.MessageTemplateTextAttribute | IncludedData.SpanIdField | IncludedData.TraceIdField)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", "HomeAccounting");
});
builder.Services.AddTransient<HttpLoggingHandler>();


builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
const string databaseName = "HomeAccounting";
builder.AddReceiptsModule(databaseName);
builder.Services.AddFnsModule();
builder.Services.AddAccountingModule();
builder.Services.AddReportsModule();

builder.Services.AddControllers();

builder.AddAuthorization(databaseName);

builder.Services.Configure<RouteOptions>(options =>
{
    options.SuppressCheckForUnhandledSecurityMetadata = true;
});

builder.Services.AddMediator((MediatorOptions options) =>
{
    options.Assemblies = [typeof(AddCheckHandler).Assembly, typeof(AddCheckCommand).Assembly, typeof(LoginQuery).Assembly];
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
}
app.UseCors(policyBuilder => policyBuilder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
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

// app.UseHttpLogging();
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "Handled {RequestMethod} {RequestPath} {StatusCode} {Elapsed}";
    options.GetLevel = SerilogApplicationBuilderExtensions.DefaultGetLevel;
});

app.MapControllers()
    .RequireAuthorization()
    ;
// app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MainLayout).Assembly)
    .AllowAnonymous();

app.Run();

namespace Api
{
    internal sealed class BearerAuthenticationSchemeTransformer(
        IAuthenticationSchemeProvider authenticationSchemeProvider)
        : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

            if (authenticationSchemes.All(scheme => scheme.Name != JwtBearerDefaults.AuthenticationScheme))
                return;

            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                {
                    JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                        BearerFormat = "JWT",
                        Description = "Please insert JWT token",
                        Name = "Authorization",
                    }
                },
                // {
                //     $"{JwtBearerDefaults.AuthenticationScheme} password", new OpenApiSecurityScheme
                //     {
                //         Type = SecuritySchemeType.Http,
                //         Scheme = "password",
                //         In = ParameterLocation.Header,
                //         
                //     }
                // }
            };

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;
            document.SecurityRequirements.Add(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        []
                    }
                }
            );
        }
    }

    internal static class SerilogApplicationBuilderExtensions
    {
        internal static LogEventLevel DefaultGetLevel(HttpContext httpContext, double elapsed, Exception? ex)
        {
            if (ex is not null || httpContext.Response.StatusCode >= 499) return LogEventLevel.Error;

            return httpContext.IsApiEndpoint()
                ? LogEventLevel.Information
                : LogEventLevel.Debug;
        }

        private static bool IsApiEndpoint(this HttpContext httpContext)
        {
            return httpContext.Request.Path.StartsWithSegments("/api");
        }
    }
}