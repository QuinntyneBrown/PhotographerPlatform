using Shared.Messages;

namespace Shared.Messaging;

public abstract class MessageHandlerBase<TMessage> where TMessage : IMessage
{
    public abstract Task HandleAsync(TMessage message, MessageContext context, CancellationToken ct);
}
