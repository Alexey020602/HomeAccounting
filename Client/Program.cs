using BlazorShared;
using BlazorShared.Api;
using Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorShared.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

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

await app.RunAsync();