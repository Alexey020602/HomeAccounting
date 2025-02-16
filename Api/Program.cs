using Api;
using Core;
using Core.Model.Report;
using Core.Model.Requests;
using Core.Services;
using DataBase;
using FnsChecksApi;
using FnsChecksApi.Dto.Fns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;
using Refit;
using SkiaSharp;
using ZXing;
using BarcodeReader = ZXing.SkiaSharp.BarcodeReader;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddOpenApi();
builder.Services.AddLogging();

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
builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
    builder.AddNpgsqlDbContext<ApplicationContext>("HomeAccounting");
else
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
app.MapOpenApi();
app.UseSwaggerUi(options =>
{
    options.DocumentPath = "openapi/v1.json";
    options.SwaggerRoutes.Add(new SwaggerUiRoute("Api", "/openapi/v1.json"));
});

app.UseCors(policyBuilder => policyBuilder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);

app.MapControllers();

app.Run();