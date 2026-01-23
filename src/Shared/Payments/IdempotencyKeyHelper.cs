using System.Security.Cryptography;
using System.Text;

namespace Shared.Payments;

public static class IdempotencyKeyHelper
{
    public static string Create(string prefix, params string[] components)
    {
        var raw = string.Join(":", components.Where(component => !string.IsNullOrWhiteSpace(component)));
        if (string.IsNullOrWhiteSpace(raw))
        {
            raw = Guid.NewGuid().ToString("N");
        }

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        return $"{prefix}_{Convert.ToHexString(hash).ToLowerInvariant()}";
    }
}
