namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class GalleryAccess
{
    public GalleryAccessLevel Level { get; set; } = GalleryAccessLevel.Public;
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public long? ExpiresAtUnixMs { get; set; }
    public string? AccessToken { get; set; }
}
