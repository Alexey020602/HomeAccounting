namespace Shared.Blazor;

public interface ILocalStorage
{
    ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default);
    ValueTask SetStringAsync(string key, string value, CancellationToken cancellationToken = default);
    ValueTask<string?> GetStringAsync(string key, CancellationToken cancellationToken = default);
}