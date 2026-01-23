using System.Security.Cryptography;

namespace Shared.Correlation;

public static class CorrelationIdGenerator
{
    public static string Create()
    {
        Span<byte> buffer = stackalloc byte[16];
        RandomNumberGenerator.Fill(buffer);
        return Convert.ToHexString(buffer).ToLowerInvariant();
    }
}
