using System.Net;

namespace Shared.Http;

public sealed class ResilientHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly HttpClientResilienceOptions _options;
    private readonly CircuitBreaker? _circuitBreaker;

    public ResilientHttpClient(HttpClient httpClient, HttpClientResilienceOptions? options = null)
    {
        _httpClient = httpClient;
        _options = options ?? new HttpClientResilienceOptions();
        _httpClient.Timeout = _options.Timeout;
        _circuitBreaker = _options.CircuitBreaker.Enabled ? new CircuitBreaker(_options.CircuitBreaker) : null;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct = default)
    {
        var attempt = 0;
        HttpRequestMessage? currentRequest = request;
        while (true)
        {
            if (_circuitBreaker is not null && !_circuitBreaker.AllowRequest())
            {
                var openUntil = _circuitBreaker.GetOpenUntilUtc();
                var message = openUntil is null
                    ? "Circuit breaker is open."
                    : $"Circuit breaker is open until {openUntil:O}.";
                throw new CircuitBreakerOpenException(message);
            }

            try
            {
                var response = await _httpClient.SendAsync(currentRequest, ct).ConfigureAwait(false);
                if (!IsTransient(response.StatusCode) || attempt >= _options.MaxRetries)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _circuitBreaker?.RecordSuccess();
                    }
                    else if (IsTransient(response.StatusCode))
                    {
                        _circuitBreaker?.RecordFailure();
                    }

                    return response;
                }

                _circuitBreaker?.RecordFailure();
                response.Dispose();
            }
            catch (Exception ex) when (IsTransient(ex) && attempt < _options.MaxRetries)
            {
                _circuitBreaker?.RecordFailure();
            }
            catch (Exception ex) when (IsTransient(ex))
            {
                _circuitBreaker?.RecordFailure();
                throw;
            }

            attempt++;
            var delay = RetryBackoff.ComputeDelay(attempt, _options);
            await Task.Delay(delay, ct).ConfigureAwait(false);
            currentRequest = await request.CloneAsync(ct).ConfigureAwait(false);
        }
    }

    private bool IsTransient(HttpStatusCode statusCode)
    {
        return _options.RetryStatusCodes.Contains((int)statusCode);
    }

    private static bool IsTransient(Exception exception)
    {
        return exception is HttpRequestException or TaskCanceledException;
    }
}
