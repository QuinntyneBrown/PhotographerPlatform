using MessagePack;

namespace Shared.Webhooks;

[MessagePackObject]
public sealed class WebhookEvent
{
    [Key(0)]
    public required string WebhookEventId { get; init; }

    [Key(1)]
    public required WebhookEventType EventType { get; init; }

    [Key(2)]
    public required string ResourceId { get; init; }

    [Key(3)]
    public required string ResourceType { get; init; }

    [Key(4)]
    public required string AccountId { get; init; }

    [Key(5)]
    public required long CreatedAtUnixMs { get; init; }

    [Key(6)]
    public required byte[] Payload { get; init; }

    [Key(7)]
    public Dictionary<string, string> Metadata { get; init; } = new();
}
