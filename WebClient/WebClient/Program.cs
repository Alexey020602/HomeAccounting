using BlazorShared;
using BlazorShared.DependencyInjection;
using BlazorShared.Layouts;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Extensions;
using WebClient.Components;


// var webAssemblyHostBuilder = WebAssemblyHostBuilder.CreateDefault(args);
// webAssemblyHostBuilder.Logging.SetMinimumLevel( webAssemblyHostBuilder.HostEnvironment.IsDevelopment() ? LogLevel.Debug: LogLevel.Information);
// webAssemblyHostBuilder.Logging.Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.None/*(ActivityTrackingOptions)127*/);
// webAssemblyHostBuilder.RootComponents.Add<Routes>("#app");
// webAssemblyHostBuilder.RootComponents.Add<HeadOutlet>("head::after");
//
// var apiUrl = webAssemblyHostBuilder.Configuration.GetValue<string>("ApiUrlHttps") ?? throw new Exception("Отсутствует адрес для api");
// var apiUri = new Uri(apiUrl);
//
// webAssemblyHostBuilder.Services.AddBlazorShared(apiUri);
// await webAssemblyHostBuilder.Build().RunAsync();

var builder = WebApplication.CreateBuilder(args);

var apiUrl = builder.Configuration.GetSection("Api").GetValue<string>("ApiUrlHttps") ?? throw new Exception("Отсутствует адрес для api");
var apiUri = new Uri(apiUrl);
// builder.Services.AddBlazorShared(apiUri);
// builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.Configure<RouteOptions>(options =>
{
    options.SuppressCheckForUnhandledSecurityMetadata = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    // .AddAdditionalAssemblies(typeof(Client._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(MainLayout).Assembly)
    // .AddAdditionalAssemblies(typeof(Client.App).Assembly)
    // .AddAdditionalAssemblies(typeof(WebClient.Client._Imports).Assembly)
    ;

app.Run();