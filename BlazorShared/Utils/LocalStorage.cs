using Microsoft.JSInterop;

namespace BlazorShared.Utils;

public sealed class LocalStorage(IJSRuntime jsRuntime): ILocalStorage
{
    private readonly IJSRuntime jsRuntime = jsRuntime;
    public ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default) => jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);

    public ValueTask SetStringAsync(string key, string value, CancellationToken cancellationToken = default) => jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, value);

    public ValueTask<string?> GetStringAsync(string key, CancellationToken cancellationToken = default) => jsRuntime.InvokeAsync<string?>("localStorage.getItem", cancellationToken, key);
}