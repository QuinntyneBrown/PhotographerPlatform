namespace Shared.Cdn;

public sealed class ImageMetadata
{
    public int Width { get; init; }
    public int Height { get; init; }
    public string? CameraModel { get; init; }
    public DateTimeOffset? CapturedAt { get; init; }
    public string? Lens { get; init; }
    public Dictionary<string, string> RawExif { get; init; } = new();
}

public interface IImageMetadataExtractor
{
    Task<ImageMetadata> ExtractAsync(Stream imageStream, CancellationToken ct = default);
}
