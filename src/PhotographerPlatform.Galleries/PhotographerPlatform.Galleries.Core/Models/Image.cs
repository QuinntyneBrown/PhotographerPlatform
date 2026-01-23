namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class Image
{
    public required string ImageId { get; init; }
    public required string GalleryId { get; init; }
    public required string FileName { get; init; }
    public required string OriginalPath { get; init; }
    public string? WebPath { get; set; }
    public string? ThumbnailSmallPath { get; set; }
    public string? ThumbnailMediumPath { get; set; }
    public string? ThumbnailLargePath { get; set; }
    public ImageMetadata Metadata { get; set; } = new();
    public int Order { get; set; }
    public DownloadPermission? DownloadOverride { get; set; }
    public long? DownloadExpiresAtUnixMs { get; set; }
    public long UploadedAtUnixMs { get; init; }
}
