namespace Shared.Http;

public sealed class CircuitBreakerOptions
{
    public bool Enabled { get; init; } = true;
    public int FailureThreshold { get; init; } = 5;
    public TimeSpan OpenDuration { get; init; } = TimeSpan.FromSeconds(30);
}
