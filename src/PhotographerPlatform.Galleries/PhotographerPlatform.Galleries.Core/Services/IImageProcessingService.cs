using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Core.Services;

public sealed class ImageProcessingOptions
{
    public int? MaxWidth { get; init; }
    public int? MaxHeight { get; init; }
    public int Quality { get; init; } = 85;
    public bool ApplyWatermark { get; init; }
    public string? WatermarkId { get; init; }
    public WatermarkPosition WatermarkPosition { get; init; } = WatermarkPosition.BottomRight;
    public double WatermarkOpacity { get; init; } = 0.6;
    public double WatermarkScale { get; init; } = 0.2;
}

public sealed class ProcessedImage
{
    public required Stream Content { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
}

public interface IImageProcessingService
{
    Task<ImageMetadata> ExtractMetadataAsync(Stream imageStream, CancellationToken ct = default);
    Task<ProcessedImage> ResizeAsync(Stream imageStream, ImageProcessingOptions options, CancellationToken ct = default);
}
