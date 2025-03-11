using Api;
using Authorization;
using Authorization.DependencyInjection;
using Core;
using Core.Model.Report;
using Core.Model.Requests;
using Core.Services;
using DataBase;
using FnsChecksApi;
using FnsChecksApi.Dto.Fns;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using NSwag.AspNetCore;
using Refit;
using SkiaSharp;
using ZXing;
using BarcodeReader = ZXing.SkiaSharp.BarcodeReader;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerAuthenticationSchemeTransformer>());
builder.Services.AddLogging();
builder.Services.AddHttpLogging(logging => logging.LoggingFields = HttpLoggingFields.All);
builder.Services.AddTransient<HttpLoggingHandler>();

builder.Services.AddRefitClient<ICheckService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"))
    .AddHttpMessageHandler<HttpLoggingHandler>();
builder.Services.AddRefitClient<IReceiptService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://cheicheck.ru"))
    .AddHttpMessageHandler<HttpLoggingHandler>();

builder.Services.AddScoped<ICheckRepository, CheckRepository>();
builder.Services.AddScoped<ICheckSource, CheckSource>();
builder.Services.AddTransient<IBarcodeReader<SKBitmap>, BarcodeReader>();
builder.Services.AddScoped<ICheckUseCase, CheckUseCase>();
builder.Services.AddScoped<IReportUseCase, ReportUseCase>();
builder.Services.AddTransient<IBarcodeService, BarcodeService>();
builder.Services.AddTransient<IAccountingSettingsService, ConfigurationAccountingSettingsService>();
builder.Services.AddControllers();

builder.Services.AddAuthorization(builder.Configuration);

if (builder.Environment.IsDevelopment())
    builder.AddNpgsqlDbContext<ApplicationContext>("HomeAccounting");
else
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();

    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "openapi/v1.json";
        options.SwaggerRoutes.Add(new SwaggerUiRoute("Api", "/openapi/v1.json"));
    });
    
}

app.UseCors(policyBuilder => policyBuilder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpLogging();
app.MapControllers()
    .RequireAuthorization()
    ;

app.Run();

internal sealed class BearerAuthenticationSchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
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