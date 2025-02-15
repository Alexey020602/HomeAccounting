using System;
using System.Reflection;
using BlazorShared.Api;
using BlazorShared.DependencyInjection;
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
        builder.Services.AddMauiBlazorWebView();
        var apiUrl =
#if DEBUG
            "http://localhost:5080";
#else
        "";
#endif
        var apiUri = new Uri(apiUrl);
        builder.Services.AddBlazorShared(apiUri);
        // builder.Services.AddMudServices();
        // builder.Services.AddRefitClient<IChecksApi>()
        //     .ConfigureHttpClient(client => client.BaseAddress = apiUri);
        // builder.Services.AddRefitClient<IReportsApi>()
        //     .ConfigureHttpClient(client => client.BaseAddress = apiUri);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}