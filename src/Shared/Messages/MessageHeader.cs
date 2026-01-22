namespace Shared.Messages;
public sealed class MessageHeader
{
    public required string MessageType { get; init; }
    public required string MessageId { get; init; }
    public required string CorrelationId { get; init; }
    public string? CausationId { get; init; }
    public long TimestampUnixMs { get; init; }
    public required string SourceService { get; init; }
    public int SchemaVersion { get; init; } = 1;
    public Dictionary<string, string> Metadata { get; init; } = new();
}
