using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Core.Services;

public sealed class DownloadResult
{
    public required Stream Content { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
}

public interface IDownloadService
{
    Task<DownloadResult> GetImageDownloadAsync(Image image, Gallery gallery, DownloadPermission permission, CancellationToken ct = default);
    Task<DownloadResult> CreateGalleryZipAsync(Gallery gallery, IReadOnlyList<Image> images, DownloadPermission permission, CancellationToken ct = default);
}
