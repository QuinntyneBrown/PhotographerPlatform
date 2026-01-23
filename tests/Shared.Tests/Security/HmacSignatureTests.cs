using Shared.Security;
using Xunit;

namespace Shared.Tests.Security;

public sealed class HmacSignatureTests
{
    [Fact]
    public void ComputeHex_ProducesExpectedSignature()
    {
        var payload = "hello";
        var secret = "secret";

        var signature = HmacSignature.ComputeHex(payload, secret);

        Assert.Equal("88aab3ede8d3adf94d26ab90d3bafd4a2083070c3bcce9c014ee04a443847c0b", signature);
    }

    [Fact]
    public void VerifyHex_ReturnsTrueForMatchingSignature()
    {
        var payload = "payload";
        var secret = "secret";
        var signature = HmacSignature.ComputeHex(payload, secret);

        var result = HmacSignature.VerifyHex(payload, secret, signature);

        Assert.True(result);
    }

    [Fact]
    public void VerifyHex_ReturnsFalseForMismatch()
    {
        var payload = "payload";
        var secret = "secret";

        var result = HmacSignature.VerifyHex(payload, secret, "deadbeef");

        Assert.False(result);
    }
}
