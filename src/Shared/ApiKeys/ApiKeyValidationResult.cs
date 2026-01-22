using MessagePack;

namespace Shared.ApiKeys;

[MessagePackObject]
public sealed class ApiKeyValidationResult
{
    [Key(0)]
    public required bool IsValid { get; init; }

    [Key(1)]
    public string? ApiKeyId { get; init; }

    [Key(2)]
    public string? AccountId { get; init; }

    [Key(3)]
    public List<string> Scopes { get; init; } = new();

    [Key(4)]
    public string? ErrorMessage { get; init; }
}
