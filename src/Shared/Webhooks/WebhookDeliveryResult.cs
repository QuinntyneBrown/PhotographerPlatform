using MessagePack;

namespace Shared.Webhooks;

[MessagePackObject]
public sealed class WebhookDeliveryResult
{
    [Key(0)]
    public required string DeliveryId { get; init; }

    [Key(1)]
    public required string WebhookEventId { get; init; }

    [Key(2)]
    public required string WebhookSubscriptionId { get; init; }

    [Key(3)]
    public required WebhookDeliveryStatus Status { get; init; }

    [Key(4)]
    public int? HttpStatusCode { get; init; }

    [Key(5)]
    public string? ResponseBody { get; init; }

    [Key(6)]
    public string? ErrorMessage { get; init; }

    [Key(7)]
    public int AttemptNumber { get; init; }

    [Key(8)]
    public long AttemptedAtUnixMs { get; init; }

    [Key(9)]
    public long? NextRetryAtUnixMs { get; init; }
}
