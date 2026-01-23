using Shared.Http;
using Xunit;

namespace Shared.Tests.Http;

public sealed class RetryBackoffTests
{
    [Fact]
    public void ComputeDelay_UsesExponentialBackoffWithoutJitter()
    {
        var options = new HttpClientResilienceOptions
        {
            BaseDelay = TimeSpan.FromMilliseconds(100),
            MaxDelay = TimeSpan.FromSeconds(10),
            UseJitter = false
        };

        var delay = RetryBackoff.ComputeDelay(2, options);

        Assert.Equal(TimeSpan.FromMilliseconds(400), delay);
    }

    [Fact]
    public void ComputeDelay_CapsAtMaxDelay()
    {
        var options = new HttpClientResilienceOptions
        {
            BaseDelay = TimeSpan.FromMilliseconds(500),
            MaxDelay = TimeSpan.FromSeconds(1),
            UseJitter = false
        };

        var delay = RetryBackoff.ComputeDelay(10, options);

        Assert.Equal(TimeSpan.FromSeconds(1), delay);
    }
}
