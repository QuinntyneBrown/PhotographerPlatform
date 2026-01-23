namespace Shared.Http;

public sealed class HttpClientResilienceOptions
{
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
    public int MaxRetries { get; init; } = 3;
    public TimeSpan BaseDelay { get; init; } = TimeSpan.FromMilliseconds(200);
    public TimeSpan MaxDelay { get; init; } = TimeSpan.FromSeconds(5);
    public bool UseJitter { get; init; } = true;
    public IReadOnlyCollection<int> RetryStatusCodes { get; init; } = new[] { 408, 429, 500, 502, 503, 504 };
    public CircuitBreakerOptions CircuitBreaker { get; init; } = new();
}
