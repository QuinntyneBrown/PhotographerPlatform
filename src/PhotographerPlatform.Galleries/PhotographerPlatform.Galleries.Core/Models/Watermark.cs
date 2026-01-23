namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class Watermark
{
    public required string WatermarkId { get; init; }
    public required string AccountId { get; init; }
    public required string FileName { get; init; }
    public required string StoragePath { get; init; }
    public long CreatedAtUnixMs { get; init; }
}
