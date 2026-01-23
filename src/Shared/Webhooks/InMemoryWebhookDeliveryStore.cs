using System.Collections.Concurrent;

namespace Shared.Webhooks;

public sealed class InMemoryWebhookDeliveryStore : IWebhookDeliveryStore
{
    private readonly ConcurrentBag<WebhookDeliveryAttempt> _attempts = new();

    public Task RecordAttemptAsync(WebhookDeliveryAttempt attempt, CancellationToken ct = default)
    {
        _attempts.Add(attempt);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<WebhookDeliveryAttempt>> GetDueAttemptsAsync(long nowUnixMs, CancellationToken ct = default)
    {
        var due = _attempts.Where(attempt => attempt.NextAttemptUnixMs.HasValue && attempt.NextAttemptUnixMs <= nowUnixMs)
            .ToList();
        return Task.FromResult<IReadOnlyList<WebhookDeliveryAttempt>>(due);
    }

    public Task MarkDeadLetterAsync(WebhookDeliveryAttempt attempt, CancellationToken ct = default)
    {
        _attempts.Add(attempt);
        return Task.CompletedTask;
    }
}
