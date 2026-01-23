namespace PhotographerPlatform.Workspace.Api.Services;

public static class RequestContext
{
    public const string AccountHeader = "X-Account-Id";
    public const string UserHeader = "X-User-Id";

    public static bool TryGetAccountId(HttpRequest request, out string accountId)
    {
        accountId = request.Headers[AccountHeader].ToString();
        if (string.IsNullOrWhiteSpace(accountId))
        {
            accountId = request.Query["accountId"].ToString();
        }

        return !string.IsNullOrWhiteSpace(accountId);
    }

    public static string GetUserId(HttpRequest request)
    {
        var userId = request.Headers[UserHeader].ToString();
        return string.IsNullOrWhiteSpace(userId) ? "system" : userId;
    }
}
