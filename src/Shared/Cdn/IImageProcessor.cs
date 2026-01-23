namespace Shared.Cdn;

public interface IImageProcessor
{
    Task<ImageProcessingResult> ResizeAsync(Stream source, ImageTransformOptions options, CancellationToken ct = default);
    Task<ImageProcessingResult> ConvertAsync(Stream source, ImageFormat format, CancellationToken ct = default);
}

public sealed class ImageProcessingResult
{
    public required Stream ImageStream { get; init; }
    public required string ContentType { get; init; }
    public long? ContentLength { get; init; }
}
