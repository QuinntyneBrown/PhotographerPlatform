namespace PhotographerPlatform.Galleries.Api.Models;

public sealed class ImageVariantResponse
{
    public required string Url { get; init; }
    public required int Width { get; init; }
    public string? Format { get; init; }
}

public sealed class ImageResponse
{
    public required string ImageId { get; init; }
    public required string FileName { get; init; }
    public required int Order { get; init; }
    public string? OriginalUrl { get; init; }
    public string? WebUrl { get; init; }
    public string? ThumbnailSmallUrl { get; init; }
    public string? ThumbnailMediumUrl { get; init; }
    public string? ThumbnailLargeUrl { get; init; }
    public List<ImageVariantResponse> Variants { get; init; } = new();
    public long UploadedAtUnixMs { get; init; }
}

public sealed class GalleryResponse
{
    public required string GalleryId { get; init; }
    public required string ProjectId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset? EventDate { get; init; }
    public string? CoverImageId { get; init; }
    public string Status { get; init; } = "";
    public object Settings { get; init; } = new();
    public List<ImageResponse> Images { get; init; } = new();
}
