namespace Shared.Webhooks;

public interface IWebhookSubscriptionRepository
{
    Task<WebhookSubscription?> GetByIdAsync(string subscriptionId, CancellationToken ct = default);
    Task<IReadOnlyList<WebhookSubscription>> GetByAccountIdAsync(string accountId, CancellationToken ct = default);
    Task<IReadOnlyList<WebhookSubscription>> GetActiveSubscriptionsForEventAsync(string accountId, WebhookEventType eventType, CancellationToken ct = default);
    Task<WebhookSubscription> CreateAsync(WebhookSubscription subscription, CancellationToken ct = default);
    Task<WebhookSubscription> UpdateAsync(WebhookSubscription subscription, CancellationToken ct = default);
    Task DeleteAsync(string subscriptionId, CancellationToken ct = default);
}
