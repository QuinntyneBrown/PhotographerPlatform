namespace Shared.Webhooks;

public sealed class WebhookDeliveryAttempt
{
    public required string DeliveryId { get; init; }
    public required string SubscriptionId { get; init; }
    public required string EventId { get; init; }
    public required int AttemptNumber { get; init; }
    public long? NextAttemptUnixMs { get; init; }
    public int? ResponseStatusCode { get; init; }
    public string? ResponseBody { get; init; }
    public string? Error { get; init; }
    public long AttemptedAtUnixMs { get; init; }
}
