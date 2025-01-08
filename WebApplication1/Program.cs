using System.Drawing;
using FnsChecksApi;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using Refit;
using ZXing;
using ZXing.CoreCompat.System.Drawing;

string corsPolicyName = "CorsPolicy";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddCors();
//TODO Добавить в Refit о
builder.Services.AddOpenApi();
builder.Services.AddRefitClient<ICheckService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"));
builder.Services.AddRefitClient<IReceiptService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://cheicheck.ru"));

builder.Services.AddTransient<IBarcodeReader<Bitmap>, BarcodeReader>();
// builder.Services.AddProblemDetails();
builder.Services.AddScoped<ICheckUseCase, CheckUseCase>();

// builder.Services.AddCors(options => 
//     options
//         .AddPolicy(
//             corsPolicyName, 
//             policyBuilder => policyBuilder
//                 .AllowAnyHeader()
//                 .AllowAnyMethod()
//                 // .WithOrigins("https://client", "http://client", "http://localhost:5160", "https://localhost:7152")
//                 .AllowAnyOrigin()
//                 ));
var app = builder.Build();
// app.UseExceptionHandler();


// app.UseCors(corsPolicyName);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseCors(policyBuilder => policyBuilder
    .WithOrigins("https://client", "http://client", "http://localhost:5160", "https://localhost:7152")
    // .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
// app.MapGet("/test", () => "Success");
// app.MapPost("test", async (context) =>
//     {
//         using var reader = new StreamReader(context.Request.Body);
//         context.Response.
//         return $"Success: {await reader.ReadToEndAsync()}";
//     }
// );
app.MapPost("/receipt", async ([FromBody] CheckRequest request, ICheckUseCase checkUseCase) =>
    {
        // var file = new FileInfo(@"C:\Users\Fedor\Downloads\photo_2025-01-01_17-37-46.jpg");
        // var request = new CheckRequest
        // {
        //     Fd = "8622",
        //     Fn = "7380440801290534",
        //     Fp = "1229409814",
        // };

        // var forecast = Enumerable.Range(1, 5).Select(index =>
        //         new WeatherForecast
        //         (
        //             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //             Random.Shared.Next(-20, 55),
        //             summaries[Random.Shared.Next(summaries.Length)]
        //         ))
        //     .ToArray();
        // return forecast;
        // return await checkService.GetAsyncByFile(file);
        // return await checkService.GetAsyncByRaw(request);
        var response = await checkUseCase.GetReceipt(request);
        return response;
    })
    // .RequireCors("My")
    .WithName("GetWeatherForecast");

app.MapPost(
        "/receiptWithFile",
        (IFormFile file, ICheckUseCase checkUseCase, IBarcodeReader<Bitmap> reader) =>
        {
            try
            {
                var bitmap = (Bitmap) Image.FromStream(file.OpenReadStream());
                var result = reader.Decode(bitmap);
                if (result == null)
                {
                    throw new ApplicationException("Failed to decode image");
                }
                
                // var tempPath = Path.Combine(environment.WebRootPath, "tmp", Path.GetRandomFileName());
                return checkUseCase.GetReceipt(new CheckRawRequest(result.Text));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        })
    .DisableAntiforgery();
// .RequireCors("My");
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