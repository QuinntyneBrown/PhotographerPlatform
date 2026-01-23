namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class Gallery
{
    public required string GalleryId { get; init; }
    public required string ProjectId { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? EventDate { get; set; }
    public string? CoverImageId { get; set; }
    public GalleryStatus Status { get; set; } = GalleryStatus.Draft;
    public GallerySettings Settings { get; set; } = new();
    public long CreatedAtUnixMs { get; init; }
    public long UpdatedAtUnixMs { get; set; }
}
