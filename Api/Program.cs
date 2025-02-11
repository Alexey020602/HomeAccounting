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

builder.Services.AddRefitClient<ICheckService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"));
builder.Services.AddRefitClient<IReceiptService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://cheicheck.ru"));

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

app.MapControllers().DisableAntiforgery();

// app.MapGet("/checks", async (IReportUseCase reportUseCase) => Results.Json(await reportUseCase.GetChecksAsync()));
// app.MapPost("/receipt", async ([FromBody] CheckRequest request, ICheckUseCase checkUseCase) =>
// {
//     var response = await checkUseCase.SaveCheck(request);
//     return Results.Json(response);
// });
//
// app.MapPost(
//         "/receiptWithFile",
//         async ([FromForm] DateTimeOffset addedDate, IFormFile file, ICheckUseCase checkUseCase,
//             IBarcodeService service) =>
//         {
//             try
//             {
//                 var stream = file.OpenReadStream();
//                 var result = await service.ReadBarcodeAsync(stream);
//
//                 return Results.Json(await checkUseCase.SaveCheck(new RawCheckRequest
//                 {
//                     QrRaw = result,
//                     AddedTime = addedDate
//                 }));
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine(ex.Message);
//                 throw;
//             }
//         })
//     .DisableAntiforgery();
app.Run();