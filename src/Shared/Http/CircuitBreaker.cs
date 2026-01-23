namespace Shared.Http;

public enum CircuitBreakerState
{
    Closed,
    Open,
    HalfOpen
}

public sealed class CircuitBreaker
{
    private readonly CircuitBreakerOptions _options;
    private readonly object _lock = new();
    private int _consecutiveFailures;
    private long _openUntilUnixMs;
    private bool _halfOpenProbeInFlight;
    private CircuitBreakerState _state = CircuitBreakerState.Closed;

    public CircuitBreaker(CircuitBreakerOptions options)
    {
        _options = options;
    }

    public CircuitBreakerState State
    {
        get
        {
            lock (_lock)
            {
                return _state;
            }
        }
    }

    public bool AllowRequest()
    {
        if (!_options.Enabled)
        {
            return true;
        }

        lock (_lock)
        {
            if (_state == CircuitBreakerState.Open)
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (now < _openUntilUnixMs)
                {
                    return false;
                }

                _state = CircuitBreakerState.HalfOpen;
                _halfOpenProbeInFlight = false;
            }

            if (_state == CircuitBreakerState.HalfOpen)
            {
                if (_halfOpenProbeInFlight)
                {
                    return false;
                }

                _halfOpenProbeInFlight = true;
                return true;
            }

            return true;
        }
    }

    public void RecordSuccess()
    {
        if (!_options.Enabled)
        {
            return;
        }

        lock (_lock)
        {
            _consecutiveFailures = 0;
            _halfOpenProbeInFlight = false;
            _state = CircuitBreakerState.Closed;
        }
    }

    public void RecordFailure()
    {
        if (!_options.Enabled)
        {
            return;
        }

        lock (_lock)
        {
            if (_state == CircuitBreakerState.HalfOpen)
            {
                Open();
                return;
            }

            _consecutiveFailures++;
            if (_consecutiveFailures >= _options.FailureThreshold)
            {
                Open();
            }
        }
    }

    public DateTimeOffset? GetOpenUntilUtc()
    {
        lock (_lock)
        {
            if (_state != CircuitBreakerState.Open)
            {
                return null;
            }

            return DateTimeOffset.FromUnixTimeMilliseconds(_openUntilUnixMs);
        }
    }

    private void Open()
    {
        _state = CircuitBreakerState.Open;
        _openUntilUnixMs = DateTimeOffset.UtcNow.Add(_options.OpenDuration).ToUnixTimeMilliseconds();
        _halfOpenProbeInFlight = false;
    }
}
