namespace Shared.Messages;
public interface IDomainEvent : IMessage
{
    string AggregateId { get; }
    string AggregateType { get; }
}
