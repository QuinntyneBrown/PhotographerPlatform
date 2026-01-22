namespace Shared.ApiKeys;

public interface IApiKeyService
{
    Task<(ApiKey Key, string PlainTextKey)> CreateAsync(string accountId, string name, List<string> scopes, long? expiresAtUnixMs = null, CancellationToken ct = default);
    Task<ApiKeyValidationResult> ValidateAsync(string plainTextKey, CancellationToken ct = default);
    Task<ApiKey?> GetByIdAsync(string apiKeyId, CancellationToken ct = default);
    Task<IReadOnlyList<ApiKey>> GetByAccountIdAsync(string accountId, CancellationToken ct = default);
    Task RevokeAsync(string apiKeyId, CancellationToken ct = default);
    Task UpdateLastUsedAsync(string apiKeyId, CancellationToken ct = default);
}
