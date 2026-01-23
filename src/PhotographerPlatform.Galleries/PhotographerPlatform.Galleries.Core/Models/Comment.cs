namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class Comment
{
    public required string CommentId { get; init; }
    public required string ImageId { get; init; }
    public required string ClientId { get; init; }
    public required string Content { get; init; }
    public string? ParentCommentId { get; init; }
    public long CreatedAtUnixMs { get; init; }
}
