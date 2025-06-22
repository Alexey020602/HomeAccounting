using Accounting.Api;
using Api;
using Authorization;
using Authorization.DependencyInjection;
using BlazorShared.Layouts;
using Checks.Api;
using Checks.Core;
using Checks.DataBase;
using Fns;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NSwag.AspNetCore;
using Reports.Contracts;
using Reports.Core;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;
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

// builder.Services.AddLogging();
// builder.Services.AddHttpLogging(logging => logging.LoggingFields = HttpLoggingFields.All);
builder.Services.AddTransient<HttpLoggingHandler>();


builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
// builder.Services.AddRefitClient<ICheckService>()
//     .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"))
//     .AddHttpMessageHandler<HttpLoggingHandler>();
// builder.Services.AddRefitClient<IReceiptService>()
//     .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://cheicheck.ru"))
//     .AddHttpMessageHandler<HttpLoggingHandler>();

builder.Services.AddScoped<ICheckRepository, CheckRepository>();
// builder.Services.AddScoped<ICheckSource, CheckSource>();

builder.Services.AddScoped<IReportUseCase, ReportUseCase>();
builder.Services.AddCheckModule();
builder.Services.AddFnsModule();
builder.Services.AddAccountingModule();

builder.Services.AddControllers()
    // .AddApplicationPart(typeof(ChecksController).Assembly)
    // .AddApplicationPart(typeof(AuthorizationController).Assembly)
    ;

builder.Services.AddAuthorization(builder.Configuration);

builder.AddDbContexts();

builder.Services.Configure<RouteOptions>(options =>
{
    options.SuppressCheckForUnhandledSecurityMetadata = true;
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var applicationContext = scope.ServiceProvider.GetRequiredService<ChecksContext>();
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