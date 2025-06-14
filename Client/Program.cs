using BlazorShared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorShared.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.SetMinimumLevel( builder.HostEnvironment.IsDevelopment() ? LogLevel.Debug: LogLevel.Information);
builder.Logging.Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.None/*(ActivityTrackingOptions)127*/);
// builder.RootComponents.Add<Routes>("#app");
// builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration.GetValue<string>("ApiUrlHttps") ?? throw new Exception("Отсутствует адрес для api");
var apiUri = new Uri(apiUrl);

builder.Services.AddBlazorShared(apiUri);

// builder.Services.AddMudServices(config =>
// {
//     config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
//     config.SnackbarConfiguration.PreventDuplicates = false;
//     config.SnackbarConfiguration.VisibleStateDuration = 4000;
// });
// builder.Services.AddRefitClient<IChecksApi>()
//     .ConfigureHttpClient(client => client.BaseAddress = apiUri);
// builder.Services.AddRefitClient<IReportsApi>()
//     .ConfigureHttpClient(client => client.BaseAddress = apiUri);

var app = builder.Build();
app.Services.GetService<ILogger<Program>>()?.LogInformation("App starting...");
Console.WriteLine("App starting...");
await app.RunAsync();