using System.Security.Claims;

namespace Shared.Auth;

public static class ClaimsPrincipalExtensions
{
    public static string? GetClaimValue(this ClaimsPrincipal principal, string claimType)
    {
        return principal.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
    }

    public static string GetRequiredClaimValue(this ClaimsPrincipal principal, string claimType)
    {
        var value = principal.GetClaimValue(claimType);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Missing required claim '{claimType}'.");
        }

        return value;
    }

    public static IReadOnlyCollection<string> GetScopes(this ClaimsPrincipal principal)
    {
        var scopeClaim = principal.GetClaimValue("scope");
        if (string.IsNullOrWhiteSpace(scopeClaim))
        {
            return Array.Empty<string>();
        }

        return scopeClaim.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    public static bool HasScope(this ClaimsPrincipal principal, string scope)
    {
        return principal.GetScopes().Contains(scope, StringComparer.OrdinalIgnoreCase);
    }

    public static bool HasAnyScope(this ClaimsPrincipal principal, params string[] scopes)
    {
        var available = principal.GetScopes();
        return scopes.Any(scope => available.Contains(scope, StringComparer.OrdinalIgnoreCase));
    }
}
