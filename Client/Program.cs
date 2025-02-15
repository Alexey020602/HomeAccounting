using BlazorShared;
using BlazorShared.Api;
using Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddRefitClient<ICheckService>()
//     .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"));
// var apiUri = new Uri("http://localhost:5080");
// var apiUri = new Uri("http://api");
// var apiUrl = builder.HostEnvironment.BaseAddress;
var apiUrl = builder.Configuration.GetValue<string>("ApiUrlHttp") ?? throw new Exception("Отсутствует адрес для api");
// var apiUrl = builder.Configuration["ApiUrlHttp"] ?? throw new Exception("Отсутствует адрес для api");
var apiUri = new Uri(apiUrl);
builder.Services.AddMudServices();
builder.Services.AddRefitClient<IChecksApi>()
    .ConfigureHttpClient(client => client.BaseAddress = apiUri);
builder.Services.AddRefitClient<IReportsApi>()
    .ConfigureHttpClient(client => client.BaseAddress = apiUri);

var app = builder.Build();

await app.RunAsync();