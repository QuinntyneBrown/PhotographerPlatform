namespace Shared.Webhooks;

public sealed class WebhookDispatchPolicy
{
    public int MaxAttempts { get; init; } = 3;
    public TimeSpan BaseDelay { get; init; } = TimeSpan.FromSeconds(2);
    public TimeSpan MaxDelay { get; init; } = TimeSpan.FromSeconds(30);
    public bool UseJitter { get; init; } = true;
}
