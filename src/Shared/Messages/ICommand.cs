namespace Shared.Messages;
public interface ICommand : IMessage
{
    string TargetId { get; }
}
