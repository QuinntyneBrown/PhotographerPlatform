using System.Text;
using System.Text.Json;

namespace Shared.Webhooks;

public sealed class WebhookEventEnvelope
{
    public required string Id { get; init; }
    public required string Type { get; init; }
    public required long OccurredAtUnixMs { get; init; }
    public required string TenantId { get; init; }
    public required string PayloadJson { get; init; }

    public static WebhookEventEnvelope FromEvent(WebhookEvent webhookEvent)
    {
        var payloadJson = Encoding.UTF8.GetString(webhookEvent.Payload);
        return new WebhookEventEnvelope
        {
            Id = webhookEvent.WebhookEventId,
            Type = webhookEvent.EventType.ToString(),
            OccurredAtUnixMs = webhookEvent.CreatedAtUnixMs,
            TenantId = webhookEvent.AccountId,
            PayloadJson = payloadJson
        };
    }

    public string Serialize()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(this, options);
    }
}
