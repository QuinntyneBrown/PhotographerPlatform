using System.Security.Cryptography;

namespace Shared.Http;

public static class RetryBackoff
{
    public static TimeSpan ComputeDelay(int attempt, HttpClientResilienceOptions options)
    {
        if (attempt < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(attempt));
        }

        var exponent = Math.Pow(2, attempt);
        var baseDelayMs = options.BaseDelay.TotalMilliseconds * exponent;
        var cappedDelayMs = Math.Min(baseDelayMs, options.MaxDelay.TotalMilliseconds);
        var jitterMs = options.UseJitter ? RandomNumberGenerator.GetInt32(0, 100) : 0;
        return TimeSpan.FromMilliseconds(cappedDelayMs + jitterMs);
    }
}
