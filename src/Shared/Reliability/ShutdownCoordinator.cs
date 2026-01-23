namespace Shared.Reliability;

public sealed class ShutdownCoordinator
{
    private readonly List<Func<CancellationToken, Task>> _callbacks = new();

    public CancellationTokenSource ShutdownTokenSource { get; } = new();

    public void Register(Func<CancellationToken, Task> callback)
    {
        _callbacks.Add(callback);
    }

    public async Task BeginShutdownAsync()
    {
        ShutdownTokenSource.Cancel();
        foreach (var callback in _callbacks)
        {
            await callback(ShutdownTokenSource.Token).ConfigureAwait(false);
        }
    }
}
