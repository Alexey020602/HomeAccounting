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
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NSwag.AspNetCore;
using Reports.Contracts;
using Reports.Core;
using Scalar.AspNetCore;
using Shared.Infrastructure;
using Shared.Utils;
using WebClient.Components;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opt =>
{
    opt.Policies.LogMessageStarting(LogLevel.Trace);
    opt.Policies.MessageExecutionLogLevel(LogLevel.Information);
    opt.Policies.MessageSuccessLogLevel(LogLevel.Information);

    opt.PublishAllMessages().ToLocalQueue("tracing").TelemetryEnabled(true);
    
    opt.Discovery
        .IncludeAssembly(typeof(LoadCheckHandler).Assembly)
        .IncludeAssembly(typeof(SaveCheckHandler).Assembly);
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();
builder.AddServiceDefaults();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerAuthenticationSchemeTransformer>());
builder.Services.AddLogging();
builder.Services.AddHttpLogging(logging =>
{
    // logging.
    logging.LoggingFields = HttpLoggingFields.All;
});
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

app.MapControllers()
    .WithHttpLogging(HttpLoggingFields.All)
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
}