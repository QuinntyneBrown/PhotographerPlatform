using Shared.Http;
using Xunit;

namespace Shared.Tests.Http;

public sealed class CircuitBreakerTests
{
    [Fact]
    public void CircuitBreaker_OpensAfterFailuresAndClosesOnSuccess()
    {
        var options = new CircuitBreakerOptions
        {
            FailureThreshold = 2,
            OpenDuration = TimeSpan.FromMilliseconds(100)
        };

        var breaker = new CircuitBreaker(options);

        breaker.RecordFailure();
        breaker.RecordFailure();

        Assert.Equal(CircuitBreakerState.Open, breaker.State);
        Assert.False(breaker.AllowRequest());

        Thread.Sleep(150);

        Assert.True(breaker.AllowRequest());
        Assert.Equal(CircuitBreakerState.HalfOpen, breaker.State);

        breaker.RecordSuccess();

        Assert.Equal(CircuitBreakerState.Closed, breaker.State);
    }
}
