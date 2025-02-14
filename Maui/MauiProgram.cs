using System;
using BlazorShared.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using MudBlazor.Services;
using Refit;

namespace Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        var appsettings =
#if DEBUG
            "wwwroot/appsettings.Development.json";
#else
            "wwwroot/appsettings.json";
#endif
            
        builder.Configuration.AddConfiguration(new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                appsettings, 
                optional: false, 
                reloadOnChange: true)
            .Build());
        builder.Services.AddMauiBlazorWebView();
        var apiUrl = builder.Configuration.GetValue<string>("ApiUrlHttp") ?? throw new Exception("Отсутствует адрес для api");
// var apiUrl = builder.Configuration["ApiUrlHttp"] ?? throw new Exception("Отсутствует адрес для api");
        var apiUri = new Uri(apiUrl);
        builder.Services.AddMudServices();
        builder.Services.AddRefitClient<IChecksApi>()
            .ConfigureHttpClient(client => client.BaseAddress = apiUri);
        builder.Services.AddRefitClient<IReportsApi>()
            .ConfigureHttpClient(client => client.BaseAddress = apiUri);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}