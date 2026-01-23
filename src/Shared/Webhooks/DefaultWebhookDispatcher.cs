using System.Net.Http.Headers;
using System.Text;
using Shared.Http;

namespace Shared.Webhooks;

public sealed class DefaultWebhookDispatcher : IWebhookDispatcher
{
    private readonly ResilientHttpClient _httpClient;
    private readonly IWebhookSubscriptionRepository _subscriptionRepository;
    private readonly IWebhookDeliveryStore _deliveryStore;
    private readonly WebhookDispatchPolicy _policy;

    public DefaultWebhookDispatcher(
        HttpClient httpClient,
        IWebhookSubscriptionRepository subscriptionRepository,
        IWebhookDeliveryStore deliveryStore,
        WebhookDispatchPolicy? policy = null)
    {
        _httpClient = new ResilientHttpClient(httpClient);
        _subscriptionRepository = subscriptionRepository;
        _deliveryStore = deliveryStore;
        _policy = policy ?? new WebhookDispatchPolicy();
    }

    public async Task<WebhookDeliveryResult> DispatchAsync(WebhookEvent webhookEvent, WebhookSubscription subscription, CancellationToken ct = default)
    {
        var envelope = WebhookEventEnvelope.FromEvent(webhookEvent);
        var payload = envelope.Serialize();
        var signature = string.IsNullOrWhiteSpace(subscription.Secret)
            ? null
            : WebhookSignature.Create(payload, subscription.Secret);

        var attemptNumber = 1;
        while (true)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, subscription.EndpointUrl)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("PhotographerPlatform", "1.0"));
            if (!string.IsNullOrWhiteSpace(signature))
            {
                request.Headers.Add("X-Webhook-Signature", signature);
            }

            try
            {
                var response = await _httpClient.SendAsync(request, ct).ConfigureAwait(false);
                var responseBody = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                var attempt = await RecordAttemptAsync(subscription, webhookEvent, attemptNumber, response, responseBody, null, ct)
                    .ConfigureAwait(false);
                var result = new WebhookDeliveryResult
                {
                    DeliveryId = attempt.DeliveryId,
                    WebhookEventId = webhookEvent.WebhookEventId,
                    WebhookSubscriptionId = subscription.WebhookSubscriptionId,
                    Status = response.IsSuccessStatusCode ? WebhookDeliveryStatus.Delivered : WebhookDeliveryStatus.Failed,
                    HttpStatusCode = (int)response.StatusCode,
                    ResponseBody = responseBody,
                    ErrorMessage = response.IsSuccessStatusCode ? null : responseBody,
                    AttemptNumber = attemptNumber,
                    AttemptedAtUnixMs = attempt.AttemptedAtUnixMs,
                    NextRetryAtUnixMs = attempt.NextAttemptUnixMs
                };

                if (response.IsSuccessStatusCode || attemptNumber >= _policy.MaxAttempts)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        await MarkDeadLetterAsync(subscription, webhookEvent, attemptNumber, response, responseBody, ct)
                            .ConfigureAwait(false);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                await RecordAttemptAsync(subscription, webhookEvent, attemptNumber, null, null, ex.Message, ct)
                    .ConfigureAwait(false);
                if (attemptNumber >= _policy.MaxAttempts)
                {
                    var deadLetterAttempt = await MarkDeadLetterAsync(subscription, webhookEvent, attemptNumber, null, ex.Message, ct)
                        .ConfigureAwait(false);
                    return new WebhookDeliveryResult
                    {
                        DeliveryId = deadLetterAttempt.DeliveryId,
                        WebhookEventId = webhookEvent.WebhookEventId,
                        WebhookSubscriptionId = subscription.WebhookSubscriptionId,
                        Status = WebhookDeliveryStatus.Failed,
                        HttpStatusCode = null,
                        ResponseBody = null,
                        ErrorMessage = ex.Message,
                        AttemptNumber = attemptNumber,
                        AttemptedAtUnixMs = deadLetterAttempt.AttemptedAtUnixMs,
                        NextRetryAtUnixMs = null
                    };
                }
            }

            attemptNumber++;
            var delay = ComputeDelay(attemptNumber);
            await Task.Delay(delay, ct).ConfigureAwait(false);
        }
    }

    public async Task<IReadOnlyList<WebhookDeliveryResult>> DispatchToAllSubscribersAsync(WebhookEvent webhookEvent, CancellationToken ct = default)
    {
        var subscriptions = await _subscriptionRepository
            .GetActiveSubscriptionsForEventAsync(webhookEvent.AccountId, webhookEvent.EventType, ct)
            .ConfigureAwait(false);

        var results = new List<WebhookDeliveryResult>();
        foreach (var subscription in subscriptions)
        {
            results.Add(await DispatchAsync(webhookEvent, subscription, ct).ConfigureAwait(false));
        }

        return results;
    }

    public async Task RetryFailedDeliveriesAsync(CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var dueAttempts = await _deliveryStore.GetDueAttemptsAsync(now, ct).ConfigureAwait(false);
        foreach (var attempt in dueAttempts)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(attempt.SubscriptionId, ct).ConfigureAwait(false);
            if (subscription is null)
            {
                continue;
            }

            var webhookEvent = new WebhookEvent
            {
                WebhookEventId = attempt.EventId,
                EventType = WebhookEventType.Unknown,
                ResourceId = string.Empty,
                ResourceType = string.Empty,
                AccountId = subscription.AccountId,
                CreatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Payload = Array.Empty<byte>()
            };

            await DispatchAsync(webhookEvent, subscription, ct).ConfigureAwait(false);
        }
    }

    private TimeSpan ComputeDelay(int attempt)
    {
        var baseDelay = _policy.BaseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1);
        var capped = Math.Min(baseDelay, _policy.MaxDelay.TotalMilliseconds);
        var jitter = _policy.UseJitter ? Random.Shared.Next(0, 250) : 0;
        return TimeSpan.FromMilliseconds(capped + jitter);
    }

    private async Task<WebhookDeliveryAttempt> RecordAttemptAsync(
        WebhookSubscription subscription,
        WebhookEvent webhookEvent,
        int attemptNumber,
        HttpResponseMessage? response,
        string? responseBody,
        string? error,
        CancellationToken ct)
    {
        var nextAttemptAt = attemptNumber >= _policy.MaxAttempts
            ? (long?)null
            : DateTimeOffset.UtcNow.Add(ComputeDelay(attemptNumber)).ToUnixTimeMilliseconds();

        var attempt = new WebhookDeliveryAttempt
        {
            DeliveryId = Guid.NewGuid().ToString("N"),
            SubscriptionId = subscription.WebhookSubscriptionId,
            EventId = webhookEvent.WebhookEventId,
            AttemptNumber = attemptNumber,
            NextAttemptUnixMs = nextAttemptAt,
            ResponseStatusCode = response is null ? null : (int)response.StatusCode,
            ResponseBody = responseBody,
            Error = error,
            AttemptedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        await _deliveryStore.RecordAttemptAsync(attempt, ct).ConfigureAwait(false);
        return attempt;
    }

    private async Task<WebhookDeliveryAttempt> MarkDeadLetterAsync(
        WebhookSubscription subscription,
        WebhookEvent webhookEvent,
        int attemptNumber,
        HttpResponseMessage? response,
        string? responseBody,
        CancellationToken ct)
    {
        var attempt = new WebhookDeliveryAttempt
        {
            DeliveryId = Guid.NewGuid().ToString("N"),
            SubscriptionId = subscription.WebhookSubscriptionId,
            EventId = webhookEvent.WebhookEventId,
            AttemptNumber = attemptNumber,
            NextAttemptUnixMs = null,
            ResponseStatusCode = response is null ? null : (int)response.StatusCode,
            ResponseBody = responseBody,
            Error = responseBody,
            AttemptedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        await _deliveryStore.MarkDeadLetterAsync(attempt, ct).ConfigureAwait(false);
        return attempt;
    }
}
