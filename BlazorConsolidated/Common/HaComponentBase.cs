using MudBlazor;

namespace BlazorConsolidated.Common;

abstract class HaComponentBase: MudComponentBase, IDisposable, IAsyncDisposable
{
    private readonly CancellationTokenSource componentCancellationTokenSource = new();
    // 0 = alive, 1 = disposed. int instead of bool, because int has Interlocked.Decrement and Increment
    private int _disposed;           
    private int _operationsCount;

    public bool IsBusy => Volatile.Read(ref _operationsCount) > 0;

    protected CancellationToken ComponentCancellationToken => componentCancellationTokenSource.Token;

    protected void ThrowIfDisposed()
        => ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) == 1, this);

    public async Task<T> BusyAsync<T>(
        Func<CancellationToken, Task<T>> action,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        using var linked = CancellationTokenSource.CreateLinkedTokenSource(componentCancellationTokenSource.Token, cancellationToken);
        var token = linked.Token;

        UpdateStateIfFirstOperation();

        try
        {
            return await action(token).ConfigureAwait(false);
        }
        finally
        {
            UpdateStateIfNoOperationsLeft();
        }
    }

    public async Task BusyAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        using var linked = CancellationTokenSource.CreateLinkedTokenSource(componentCancellationTokenSource.Token, cancellationToken);
        var token = linked.Token;

        UpdateStateIfFirstOperation();

        try
        {
            await action(token).ConfigureAwait(false);
        }
        finally
        {
            UpdateStateIfNoOperationsLeft();
        }
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 1)
            return;

        try { componentCancellationTokenSource.Cancel(); } catch { /* ignore */ }
        componentCancellationTokenSource.Dispose();

        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    private void UpdateStateIfNoOperationsLeft()
    {
        var left = Interlocked.Decrement(ref _operationsCount);
        if (left == 0 && Volatile.Read(ref _disposed) == 0)
            _ = InvokeAsync(StateHasChanged);
    }

    private void UpdateStateIfFirstOperation()
    {
        var newCount = Interlocked.Increment(ref _operationsCount);
        if (newCount == 1)
            _ = InvokeAsync(StateHasChanged);
    }
}