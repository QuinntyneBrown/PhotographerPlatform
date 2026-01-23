using System.Security.Cryptography;
using System.Text;

namespace Shared.ApiKeys;

public static class ApiKeyHasher
{
    public static string Hash(string plainTextKey, string salt)
    {
        var bytes = Encoding.UTF8.GetBytes($"{plainTextKey}:{salt}");
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public static bool Verify(string plainTextKey, string salt, string expectedHash)
    {
        var computed = Hash(plainTextKey, salt);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computed),
            Encoding.UTF8.GetBytes(expectedHash));
    }
}
