using Microsoft.JSInterop;
using Shared.Blazor;

namespace BlazorConsolidated.Utils;

public sealed class LocalStorage(IJSRuntime jsRuntime): ILocalStorage
{
    private readonly IJSRuntime jsRuntime = jsRuntime;
    public async ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default) => await jsRuntime.InvokeAsync<string>("localStorage.removeItem", cancellationToken, key);

    public ValueTask SetStringAsync(string key, string value, CancellationToken cancellationToken = default) => jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, value);

    public async ValueTask<string?> GetStringAsync(string key, CancellationToken cancellationToken = default) => await jsRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken, key);
}