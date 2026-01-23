using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Core.Services;

public interface IGalleriesService
{
    Task<Gallery> CreateGalleryAsync(string projectId, string name, string? description, DateTimeOffset? eventDate, CancellationToken ct = default);
    Task<Gallery?> GetGalleryAsync(string galleryId, CancellationToken ct = default);
    Task<Gallery> UpdateGalleryAsync(string galleryId, string name, string? description, DateTimeOffset? eventDate, CancellationToken ct = default);
    Task ArchiveGalleryAsync(string galleryId, CancellationToken ct = default);
    Task DeleteGalleryAsync(string galleryId, CancellationToken ct = default);
    Task SetCoverImageAsync(string galleryId, string imageId, CancellationToken ct = default);

    Task<IReadOnlyList<Image>> AddImagesAsync(string galleryId, IReadOnlyList<ImageUploadDescriptor> uploads, CancellationToken ct = default);
    Task ReorderImagesAsync(string galleryId, IReadOnlyList<string> orderedImageIds, CancellationToken ct = default);
    Task DeleteImageAsync(string imageId, CancellationToken ct = default);
    Task UpdateImageMetadataAsync(string imageId, ImageMetadata metadata, CancellationToken ct = default);

    Task<ImageSet> CreateImageSetAsync(string galleryId, string name, IReadOnlyList<string> imageIds, CancellationToken ct = default);
    Task UpdateImageSetImagesAsync(string setId, IReadOnlyList<string> imageIds, CancellationToken ct = default);
    Task DeleteImageSetAsync(string setId, CancellationToken ct = default);

    Task<FavoriteToggleResult> ToggleFavoriteAsync(string galleryId, string imageId, string clientId, CancellationToken ct = default);
    Task<IReadOnlyList<Favorite>> ListFavoritesAsync(string galleryId, string clientId, CancellationToken ct = default);

    Task<Comment> AddCommentAsync(string imageId, string clientId, string content, string? parentCommentId, CancellationToken ct = default);
    Task<IReadOnlyList<Comment>> ListCommentsAsync(string imageId, CancellationToken ct = default);
    Task DeleteCommentAsync(string commentId, CancellationToken ct = default);

    Task SetAccessLevelAsync(string galleryId, GalleryAccessLevel level, CancellationToken ct = default);
    Task SetAccessPasswordAsync(string galleryId, string password, CancellationToken ct = default);
    Task<AccessVerificationResult> VerifyAccessAsync(string galleryId, string password, CancellationToken ct = default);
    Task SetAccessExpirationAsync(string galleryId, DateTimeOffset? expiresAt, CancellationToken ct = default);

    Task SetDownloadSettingsAsync(string galleryId, DownloadSettings settings, CancellationToken ct = default);
    Task<DownloadPermission> GetDownloadPermissionAsync(string galleryId, string imageId, CancellationToken ct = default);
    Task SetProofingSettingsAsync(string galleryId, ProofingSettings settings, CancellationToken ct = default);

    Task<Watermark> UploadWatermarkAsync(string accountId, string fileName, string storagePath, CancellationToken ct = default);
    Task<IReadOnlyList<Watermark>> ListWatermarksAsync(string accountId, CancellationToken ct = default);
    Task DeleteWatermarkAsync(string watermarkId, CancellationToken ct = default);
    Task SetGalleryWatermarkAsync(string galleryId, WatermarkSettings settings, CancellationToken ct = default);
}

public sealed class ImageUploadDescriptor
{
    public required string FileName { get; init; }
    public required string OriginalPath { get; init; }
    public string? WebPath { get; init; }
    public string? ThumbnailSmallPath { get; init; }
    public string? ThumbnailMediumPath { get; init; }
    public string? ThumbnailLargePath { get; init; }
    public ImageMetadata Metadata { get; init; } = new();
}

public sealed class FavoriteToggleResult
{
    public required bool IsFavorited { get; init; }
    public required int FavoriteCount { get; init; }
}

public sealed class AccessVerificationResult
{
    public required bool IsAuthorized { get; init; }
    public string? AccessToken { get; init; }
}
