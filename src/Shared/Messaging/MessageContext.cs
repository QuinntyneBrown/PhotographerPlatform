using Shared.Messages;

namespace Shared.Messaging;

public sealed class MessageContext
{
    public required MessageHeader Header { get; init; }
}
