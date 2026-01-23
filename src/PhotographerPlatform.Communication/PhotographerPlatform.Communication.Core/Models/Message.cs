namespace PhotographerPlatform.Communication.Core.Models;

public sealed class Message
{
    public required string MessageId { get; init; }
    public required string ProjectId { get; init; }
    public required string SenderId { get; init; }
    public required string Content { get; set; }
    public MessageVisibility Visibility { get; set; } = MessageVisibility.Internal;
    public string? ThreadId { get; set; }
    public bool IsRead { get; set; }
    public long CreatedAtUnixMs { get; init; }
    public long? UpdatedAtUnixMs { get; set; }
}
