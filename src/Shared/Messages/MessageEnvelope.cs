namespace Shared.Messages;
public sealed class MessageEnvelope<TPayload> where TPayload : IMessage
{
    public required MessageHeader Header { get; init; }
    public required TPayload Payload { get; init; }
}
