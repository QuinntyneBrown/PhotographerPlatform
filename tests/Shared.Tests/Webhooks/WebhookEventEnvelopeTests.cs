using System.Text;
using System.Text.Json;
using Shared.Webhooks;
using Xunit;

namespace Shared.Tests.Webhooks;

public sealed class WebhookEventEnvelopeTests
{
    [Fact]
    public void Serialize_UsesCamelCaseAndIncludesPayload()
    {
        var payload = Encoding.UTF8.GetBytes("{\"orderId\":\"order_1\"}");
        var webhookEvent = new WebhookEvent
        {
            WebhookEventId = "evt_1",
            EventType = WebhookEventType.OrderCreated,
            ResourceId = "order_1",
            ResourceType = "order",
            AccountId = "acct_1",
            CreatedAtUnixMs = 1234567890,
            Payload = payload
        };

        var envelope = WebhookEventEnvelope.FromEvent(webhookEvent);
        var json = envelope.Serialize();

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal("evt_1", root.GetProperty("id").GetString());
        Assert.Equal("OrderCreated", root.GetProperty("type").GetString());
        Assert.Equal(1234567890, root.GetProperty("occurredAtUnixMs").GetInt64());
        Assert.Equal("acct_1", root.GetProperty("tenantId").GetString());
        Assert.Equal("{\"orderId\":\"order_1\"}", root.GetProperty("payloadJson").GetString());
    }
}
