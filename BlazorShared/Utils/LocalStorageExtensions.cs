using System.Text.Json;

namespace BlazorShared.Utils;

public static class LocalStorageExtensions
{
    public static ValueTask SetAsync<T>(this ILocalStorage storage, string key, T value, CancellationToken cancellationToken) =>
        storage.SetStringAsync(key, JsonSerializer.Serialize(value), cancellationToken);

    public static async ValueTask<T?> GetAsync<T>(this ILocalStorage storage, string key, CancellationToken cancellationToken)
    {
        var value = await storage.GetStringAsync(key, cancellationToken);
        return value is null ? default : JsonSerializer.Deserialize<T>(value);
    }
}