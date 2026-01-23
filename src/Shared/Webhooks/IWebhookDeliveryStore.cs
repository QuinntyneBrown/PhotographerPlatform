namespace Shared.Webhooks;

public interface IWebhookDeliveryStore
{
    Task RecordAttemptAsync(WebhookDeliveryAttempt attempt, CancellationToken ct = default);
    Task<IReadOnlyList<WebhookDeliveryAttempt>> GetDueAttemptsAsync(long nowUnixMs, CancellationToken ct = default);
    Task MarkDeadLetterAsync(WebhookDeliveryAttempt attempt, CancellationToken ct = default);
}
