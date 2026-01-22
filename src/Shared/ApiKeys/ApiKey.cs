using MessagePack;

namespace Shared.ApiKeys;

[MessagePackObject]
public sealed class ApiKey
{
    [Key(0)]
    public required string ApiKeyId { get; init; }

    [Key(1)]
    public required string AccountId { get; init; }

    [Key(2)]
    public required string Name { get; init; }

    [Key(3)]
    public required string KeyPrefix { get; init; }

    [Key(4)]
    public required string HashedKey { get; init; }

    [Key(5)]
    public required List<string> Scopes { get; init; }

    [Key(6)]
    public bool IsActive { get; init; } = true;

    [Key(7)]
    public long CreatedAtUnixMs { get; init; }

    [Key(8)]
    public long? ExpiresAtUnixMs { get; init; }

    [Key(9)]
    public long? LastUsedAtUnixMs { get; init; }
}
