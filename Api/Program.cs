using Accounting.Api;
using Api;
using Authorization;
using Authorization.DependencyInjection;
using Checks.Api;
using Checks.Core;
using Checks.DataBase;
using Checks.DataBase.Entities;
using Fns;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NSwag.AspNetCore;
using Refit;
using Reports.Contracts;
using Reports.Core;
using Scalar.AspNetCore;
using Shared.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.AddServiceDefaults();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerAuthenticationSchemeTransformer>());
builder.Services.AddLogging();
builder.Services.AddHttpLogging(logging => logging.LoggingFields = HttpLoggingFields.All);
builder.Services.AddTransient<HttpLoggingHandler>();

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

if (builder.Environment.IsDevelopment())
{
    builder.AddNpgsqlDbContext<ChecksContext>("HomeAccounting");
    builder.AddNpgsqlDbContext<AuthorizationContext>(
        "HomeAccounting",
        configureDbContextOptions: options => options.SetUpAuthorizationForDevelopment()
    );
}
else
{
    builder.Services.AddDbContext<ChecksContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddDbContext<AuthorizationContext>(options =>
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            .SetUpAuthorization()
    );
}

var app = builder.Build();

app.UseCors(policyBuilder => policyBuilder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
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

app.UseHttpLogging();

app.MapControllers()
    .RequireAuthorization()
    ;

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