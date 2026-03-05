using BlazorConsolidated.Common.Logout;
using BlazorConsolidated.Users.Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BlazorConsolidated.Common;

public interface IFileDownloader
{
    Task DownloadFileAsync(
        string url, 
        // string suggestedFileName,
        FileDownloadOptions? options = null, 
        CancellationToken cancellationToken = default);
}

internal sealed class BrowserFileDownloader(IJSRuntime jsRuntime) : IFileDownloader
{
    public async Task DownloadFileAsync(string url, FileDownloadOptions? options = null, CancellationToken cancellationToken = default)
    {
        await jsRuntime.InvokeVoidAsync("open", cancellationToken, url);
    }
}

// internal sealed class BrowserFileDownloader(ITokenService authenticationProvider, ILogoutService logoutService, IJSRuntime jsRuntime): IFileDownloader
// {
//     public async Task DownloadFileAsync(
//         string url,
//         string suggestedFileName, 
//         FileDownloadOptions? options = null,
//         CancellationToken cancellationToken = default)
//     {
//        var headers = new Dictionary<string, string>();
//         var downloadOptions = options ?? new FileDownloadOptions();
//        const string scheme = "Bearer";
//        if (downloadOptions is { RequireAuth: true })
//        {
//            var token = await authenticationProvider.GetFreshAccessToken(cancellationToken);
//            if (token is null)
//            {
//                await logoutService.Logout(cancellationToken);
//                throw new InvalidOperationException("Authentication Failed");
//            }
//            var authentication = $"{scheme} {token}";
//            headers.Add("Authorization", authentication);
//        }
//
//        var progressProviderRef = DotNetObjectReference.Create(new BrowserFileDownloadHelper(downloadOptions.Progress));
//
//        try
//        {
//            await DownloadFileAsync(url, suggestedFileName, headers, progressProviderRef, cancellationToken);
//        }
//        catch (JSException ex) when (IsUnauthorizedException(ex, downloadOptions))
//        {
//            var token = await authenticationProvider.GetRefreshedToken(cancellationToken);
//            var newAuthentication = $"{scheme} {token}";
//            headers["Authorization"] = newAuthentication;
//            try
//            {
//                await DownloadFileAsync(url, suggestedFileName, headers, progressProviderRef, cancellationToken);
//            }
//            catch //(JSException secondEx) when (IsUnauthorizedException(secondEx, options))
//            {
//                await logoutService.Logout(cancellationToken);
//            }
//        }
//        finally
//        {
//            progressProviderRef.Dispose();
//        }
//
//        
//     }
//
//     private static bool IsUnauthorizedException(JSException ex, FileDownloadOptions? options)
//     {
//         return (ex.Message.Contains("401") || ex.Message.Contains("Unauthorized")) &&
//                options is { RequireAuth: true };
//     }
//
//     private async ValueTask DownloadFileAsync(string url, string suggestedFileName, Dictionary<string, string> headers, DotNetObjectReference<BrowserFileDownloadHelper> helperRef, CancellationToken cancellationToken)
//     {
//         await jsRuntime.InvokeVoidAsync("DownloadFile", cancellationToken, url, suggestedFileName, headers, helperRef);
//     }
// }
//
// internal sealed class BrowserFileDownloadHelper(IProgress<DownloadProgress>? progress)
// {
//     [JSInvokable]
//     public void ReportProgress(long loaded, long total)
//     {
//         progress?.Report(new DownloadProgress(loaded, total));
//     } 
// }
// public readonly record struct DownloadProgress(long Loaded, long? Total);

public readonly record struct FileDownloadOptions(bool RequireAuth = false);
internal static class BrowserFileDownloadDependencyInjection
{
    extension(IServiceCollection services)
    {
        public void AddBrowserFileDownloader()
        {
            services.AddScoped<IFileDownloader, BrowserFileDownloader>();
        }
    } 
}