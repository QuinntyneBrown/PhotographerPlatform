namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class Favorite
{
    public required string FavoriteId { get; init; }
    public required string GalleryId { get; init; }
    public required string ImageId { get; init; }
    public required string ClientId { get; init; }
    public long CreatedAtUnixMs { get; init; }
}
