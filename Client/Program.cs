using BlazorConsolidated;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorConsolidated.DependencyInjection;
using ClientServerShared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.SetMinimumLevel( builder.HostEnvironment.IsDevelopment() ? LogLevel.Debug: LogLevel.Information);
builder.Logging.Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.None/*(ActivityTrackingOptions)127*/);

var apiUrl = builder.HostEnvironment.BaseAddress;
var apiUri = new Uri(apiUrl).AppendingPath("api");

builder.Services.AddBlazorShared(apiUri);

var app = builder.Build();
app.Services.GetService<ILogger<Program>>()?.LogInformation("App starting...");
await app.RunAsync();