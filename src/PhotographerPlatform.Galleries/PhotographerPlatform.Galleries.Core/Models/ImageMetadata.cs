namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class ImageMetadata
{
    public int Width { get; init; }
    public int Height { get; init; }
    public string? CameraModel { get; init; }
    public string? Lens { get; init; }
    public long? CapturedAtUnixMs { get; init; }
    public Dictionary<string, string> Raw { get; init; } = new();
}
