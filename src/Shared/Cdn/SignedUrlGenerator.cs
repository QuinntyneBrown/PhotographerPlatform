using Shared.Security;

namespace Shared.Cdn;

public sealed class SignedUrlGenerator
{
    private readonly string _secret;

    public SignedUrlGenerator(string secret)
    {
        _secret = secret;
    }

    public string SignUrl(string url, long expiresAtUnixMs)
    {
        var uri = new Uri(url);
        var queryValues = ParseQuery(uri.Query);
        queryValues["exp"] = expiresAtUnixMs.ToString();
        var unsigned = $"{uri.GetLeftPart(UriPartial.Path)}?{BuildQuery(queryValues)}";
        var signature = HmacSignature.ComputeHex(unsigned, _secret);
        return $"{unsigned}&sig={signature}";
    }

    private static Dictionary<string, string> ParseQuery(string query)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(query))
        {
            return result;
        }

        var trimmed = query.TrimStart('?');
        var pairs = trimmed.Split('&', StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2);
            var key = Uri.UnescapeDataString(parts[0]);
            var value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : string.Empty;
            result[key] = value;
        }

        return result;
    }

    private static string BuildQuery(Dictionary<string, string> values)
    {
        return string.Join("&", values.Select(kvp =>
            $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
    }
}
