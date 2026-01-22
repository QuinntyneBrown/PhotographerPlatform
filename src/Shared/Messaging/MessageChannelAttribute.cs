namespace Shared.Messaging;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class MessageChannelAttribute : Attribute
{
    public MessageChannelAttribute(string domain, string aggregate, string eventType, int version)
    {
        Domain = domain;
        Aggregate = aggregate;
        EventType = eventType;
        Version = version;
    }

    public string Domain { get; }
    public string Aggregate { get; }
    public string EventType { get; }
    public int Version { get; }

    public string ChannelName => $"{Domain}.{Aggregate}.{EventType}.v{Version}";
}
