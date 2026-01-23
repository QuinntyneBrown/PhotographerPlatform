namespace PhotographerPlatform.Galleries.Api.Models;

public sealed class ImageOrderRequest
{
    public required List<string> ImageIds { get; init; }
}

public sealed class ImageSetCreateRequest
{
    public required string Name { get; init; }
    public List<string> ImageIds { get; init; } = new();
}

public sealed class ImageSetUpdateRequest
{
    public required List<string> ImageIds { get; init; }
}

public sealed class FavoriteRequest
{
    public string? ClientId { get; init; }
}

public sealed class CommentRequest
{
    public string? ClientId { get; init; }
    public required string Content { get; init; }
    public string? ParentCommentId { get; init; }
}

public sealed class ProofingUpdateRequest
{
    public bool? Enabled { get; init; }
    public int? SelectionLimit { get; init; }
    public DateTimeOffset? Deadline { get; init; }
}
