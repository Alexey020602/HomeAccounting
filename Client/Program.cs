using Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddRefitClient<ICheckService>()
//     .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"));
// var apiUri = new Uri("http://localhost:5080");
// var apiUri = new Uri("http://api");
// var apiUrl = builder.HostEnvironment.BaseAddress;
var apiUrl = builder.Configuration.GetValue<string>("ApiUrlHttp") ?? throw new Exception("Отсутствует адрес для api");
// var apiUrl = builder.Configuration["ApiUrlHttp"] ?? throw new Exception("Отсутствует адрес для api");
var apiUri = new Uri(apiUrl);
builder.Services.AddRefitClient<IChecksApi>()
    .ConfigureHttpClient(client => client.BaseAddress = apiUri);
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var app = builder.Build();

await app.RunAsync();