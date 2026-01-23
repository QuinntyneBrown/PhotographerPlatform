using System.Security.Claims;

namespace Shared.Auth;

public sealed class PolicyRequirement
{
    public IReadOnlyCollection<string> RequiredScopes { get; init; } = Array.Empty<string>();
    public IReadOnlyCollection<string> RequiredRoles { get; init; } = Array.Empty<string>();
    public bool RequireAllScopes { get; init; } = true;

    public bool IsSatisfiedBy(ClaimsPrincipal principal)
    {
        if (RequiredRoles.Any(role => !principal.IsInRole(role)))
        {
            return false;
        }

        if (RequiredScopes.Count == 0)
        {
            return true;
        }

        return RequireAllScopes
            ? RequiredScopes.All(principal.HasScope)
            : RequiredScopes.Any(principal.HasScope);
    }
}
