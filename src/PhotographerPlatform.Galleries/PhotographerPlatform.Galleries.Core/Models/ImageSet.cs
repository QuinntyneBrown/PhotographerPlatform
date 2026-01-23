namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class ImageSet
{
    public required string ImageSetId { get; init; }
    public required string GalleryId { get; init; }
    public required string Name { get; set; }
    public List<string> ImageIds { get; set; } = new();
    public int Order { get; set; }
    public long CreatedAtUnixMs { get; init; }
}
