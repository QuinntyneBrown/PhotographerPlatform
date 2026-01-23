using System.Security.Cryptography;

namespace Shared.ApiKeys;

public static class ApiKeyGenerator
{
    public static string GeneratePlainText(string prefix = "ppk")
    {
        Span<byte> buffer = stackalloc byte[32];
        RandomNumberGenerator.Fill(buffer);
        var token = Convert.ToHexString(buffer).ToLowerInvariant();
        return $"{prefix}_{token}";
    }
}
