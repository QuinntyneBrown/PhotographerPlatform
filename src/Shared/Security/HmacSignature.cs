using System.Security.Cryptography;
using System.Text;

namespace Shared.Security;

public static class HmacSignature
{
    public static string ComputeHex(string payload, string secret)
    {
        var payloadBytes = Encoding.UTF8.GetBytes(payload);
        var secretBytes = Encoding.UTF8.GetBytes(secret);
        using var hmac = new HMACSHA256(secretBytes);
        var hash = hmac.ComputeHash(payloadBytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public static bool VerifyHex(string payload, string secret, string expectedSignature)
    {
        if (string.IsNullOrWhiteSpace(expectedSignature))
        {
            return false;
        }

        var actual = ComputeHex(payload, secret);
        return FixedTimeEquals(actual, expectedSignature);
    }

    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
