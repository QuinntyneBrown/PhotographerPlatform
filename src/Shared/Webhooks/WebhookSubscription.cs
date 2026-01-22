using MessagePack;

namespace Shared.Webhooks;

[MessagePackObject]
public sealed class WebhookSubscription
{
    [Key(0)]
    public required string WebhookSubscriptionId { get; init; }

    [Key(1)]
    public required string AccountId { get; init; }

    [Key(2)]
    public required string EndpointUrl { get; init; }

    [Key(3)]
    public required string Secret { get; init; }

    [Key(4)]
    public required List<WebhookEventType> SubscribedEvents { get; init; }

    [Key(5)]
    public bool IsActive { get; init; } = true;

    [Key(6)]
    public long CreatedAtUnixMs { get; init; }

    [Key(7)]
    public string? Description { get; init; }
}
