namespace Shared.ApiKeys;

public sealed class ApiKeyRotationPlan
{
    public required ApiKey ExistingKey { get; init; }
    public required ApiKey NewKey { get; init; }
    public required string PlainTextNewKey { get; init; }
}
