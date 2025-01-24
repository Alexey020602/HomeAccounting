using System.Drawing;
using Core;
using DataBase;
using FnsChecksApi;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Requests;
using Microsoft.AspNetCore.Mvc;
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

// builder.Services.AddProblemDetails();

if (builder.Environment.IsDevelopment())
{
    
    
    builder.AddNpgsqlDbContext<ApplicationContext>("HomeAccounting");
}
else
{
    //todo Сделать регистрацию БД из ConnectionStrings
}

var app = builder.Build();

// app.UseExceptionHandler(new ExceptionHandlerOptions()
// {
// });

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

// app.UseHttpsRedirection();
app.UseCors(policyBuilder => policyBuilder
    // .WithOrigins("https://client", "http://client", "http://localhost:5160", "https://localhost:7152")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/checks", async (IReportUseCase reportUseCase) => Results.Json(await reportUseCase.GetChecksAsync()));
app.MapPost("/receipt", async ([FromBody] CheckRequest request, ICheckUseCase checkUseCase) =>
    {
        var response = await checkUseCase.GetReceipt(request);
        return response;
    });

app.MapPost(
        "/receiptWithFile",
        async (IFormFile file, ICheckUseCase checkUseCase, IBarcodeReader<SKBitmap> reader) =>
        {
            try
            {
                var stream = file.OpenReadStream();
                var memory = new byte[stream.Length];
                await stream.ReadExactlyAsync(memory, 0, memory.Length);
                var image = SKImage.FromEncodedData(memory);
                var bitmap = SKBitmap.FromImage(image);
                var result = reader.Decode(bitmap);
                if (result == null)
                {
                    return Results.BadRequest("Не удалось обработать изображение");
                }

                return Results.Json(await checkUseCase.SaveCheck(new CheckRawRequest(result.Text)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        })
    .DisableAntiforgery();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public static class CheckUseCaseExtensions
{
    public static Task<Root> GetReceiptAsync(this ICheckUseCase checkUseCase, IFormFile file, string path)
    {
        var fileStream = new FileStream(path, FileMode.Create);
        file.CopyTo(fileStream);
        return checkUseCase.GetReceipt(fileStream);
    }
}