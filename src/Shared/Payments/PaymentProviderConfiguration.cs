namespace Shared.Payments;

public sealed class PaymentProviderConfiguration
{
    public required string ProviderName { get; init; }
    public required string ApiKey { get; init; }
    public string? WebhookSecret { get; init; }
    public string Environment { get; init; } = "Sandbox";
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
    public Dictionary<string, string> Metadata { get; init; } = new();
}
