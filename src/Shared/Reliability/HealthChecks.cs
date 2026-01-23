namespace Shared.Reliability;

public interface IHealthCheck
{
    string Name { get; }
    Task<HealthCheckResult> CheckAsync(CancellationToken ct = default);
}

public sealed class HealthCheckResult
{
    public required string Name { get; init; }
    public required bool IsHealthy { get; init; }
    public string? Message { get; init; }
    public long DurationMs { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = new();
}

public sealed class HealthCheckProbe : IHealthCheck
{
    private readonly Func<CancellationToken, Task<bool>> _probe;
    private readonly Func<Task<string?>>? _messageFactory;

    public HealthCheckProbe(string name, Func<CancellationToken, Task<bool>> probe, Func<Task<string?>>? messageFactory = null)
    {
        Name = name;
        _probe = probe;
        _messageFactory = messageFactory;
    }

    public string Name { get; }

    public async Task<HealthCheckResult> CheckAsync(CancellationToken ct = default)
    {
        var started = DateTimeOffset.UtcNow;
        var isHealthy = await _probe(ct).ConfigureAwait(false);
        var message = _messageFactory is null ? null : await _messageFactory().ConfigureAwait(false);
        var duration = (long)(DateTimeOffset.UtcNow - started).TotalMilliseconds;
        return new HealthCheckResult
        {
            Name = Name,
            IsHealthy = isHealthy,
            Message = message,
            DurationMs = duration
        };
    }
}

public static class HealthCheckRegistry
{
    public static async Task<IReadOnlyList<HealthCheckResult>> RunAsync(IEnumerable<IHealthCheck> checks, CancellationToken ct = default)
    {
        var results = new List<HealthCheckResult>();
        foreach (var check in checks)
        {
            results.Add(await check.CheckAsync(ct).ConfigureAwait(false));
        }

        return results;
    }
}
