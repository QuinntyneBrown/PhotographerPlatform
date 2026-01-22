namespace Shared.Webhooks;

public interface IWebhookDispatcher
{
    Task<WebhookDeliveryResult> DispatchAsync(WebhookEvent webhookEvent, WebhookSubscription subscription, CancellationToken ct = default);
    Task<IReadOnlyList<WebhookDeliveryResult>> DispatchToAllSubscribersAsync(WebhookEvent webhookEvent, CancellationToken ct = default);
    Task RetryFailedDeliveriesAsync(CancellationToken ct = default);
}
