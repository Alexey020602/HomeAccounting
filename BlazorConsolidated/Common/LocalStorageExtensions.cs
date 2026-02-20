using System.Text.Json;

namespace BlazorConsolidated.Common;

public static class LocalStorageExtensions
{
    extension(ILocalStorage storage)
    {
        public ValueTask SetAsync<T>(string key, T value, JsonSerializerOptions? options = null,  CancellationToken cancellationToken = default) =>
            storage.SetStringAsync(key, JsonSerializer.Serialize(value, options), cancellationToken);

        public async ValueTask<T?> GetAsync<T>(string key, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            var value = await storage.GetStringAsync(key, cancellationToken);
            return value is null ? default : JsonSerializer.Deserialize<T>(value, options);
        }
    }
}