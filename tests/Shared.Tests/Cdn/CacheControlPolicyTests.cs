using Shared.Cdn;
using Xunit;

namespace Shared.Tests.Cdn;

public sealed class CacheControlPolicyTests
{
    [Fact]
    public void ToHeaderValue_FormatsDirectives()
    {
        var policy = new CacheControlPolicy
        {
            Public = false,
            MaxAgeSeconds = 60,
            SharedMaxAgeSeconds = 120,
            Immutable = true,
            NoTransform = false
        };

        var header = policy.ToHeaderValue();

        Assert.Equal("private, max-age=60, s-maxage=120, immutable", header);
    }
}
