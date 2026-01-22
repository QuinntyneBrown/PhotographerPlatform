namespace Shared.Cdn;

public interface ICdnService
{
    Task<ImageUploadResult> UploadImageAsync(Stream imageStream, string fileName, string contentType, CancellationToken ct = default);
    Task<string> GetTransformedUrlAsync(string imageId, ImageTransformOptions options, CancellationToken ct = default);
    Task<ResponsiveImage> GetResponsiveImageAsync(string imageId, int[] widths, ImageFormat format = ImageFormat.WebP, CancellationToken ct = default);
    Task DeleteImageAsync(string imageId, CancellationToken ct = default);
    Task PurgeFromCacheAsync(string imageId, CancellationToken ct = default);
}
