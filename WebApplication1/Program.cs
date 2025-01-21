using System.Drawing;
using System.Text.Json;
using DataBase;
using FnsChecksApi;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using Refit;
using ZXing;
using ZXing.CoreCompat.System.Drawing;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddOpenApi();

builder.Services.AddRefitClient<ICheckService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"));
builder.Services.AddRefitClient<IReceiptService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://cheicheck.ru"));

builder.Services.AddTransient<IBarcodeReader<Bitmap>, BarcodeReader>();
builder.Services.AddScoped<ICheckUseCase, CheckUseCase>();

if (builder.Environment.IsDevelopment())
{
    builder.AddNpgsqlDbContext<ApplicationContext>("HomeAccounting");
}
else
{
    //todo Сделать регистрацию БД из ConnectionStrings
}

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseCors(policyBuilder => policyBuilder
    .WithOrigins("https://client", "http://client", "http://localhost:5160", "https://localhost:7152")
    .AllowAnyHeader()
    .AllowAnyMethod()
);
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
app.MapPost("/receipt", async ([FromBody] CheckRequest request, ICheckUseCase checkUseCase) =>
    {
        var response = await checkUseCase.GetReceipt(request);
        return response;
    })
    .WithName("GetWeatherForecast");

app.MapPost(
        "/receiptWithFile",
        async (IFormFile file, ICheckUseCase checkUseCase, IBarcodeReader<Bitmap> reader) =>
        {
            try
            {
                var bitmap = (Bitmap) Image.FromStream(file.OpenReadStream());
                var result = reader.Decode(bitmap);
                if (result == null)
                {
                    return Results.BadRequest("Не удалось обработать изображение");
                }

                return Results.Json(await checkUseCase.GetReceipt(new CheckRawRequest(result.Text)));
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