using MessagePack;

namespace Shared.Cdn;

[MessagePackObject]
public sealed class ImageUploadResult
{
    [Key(0)]
    public required string ImageId { get; init; }

    [Key(1)]
    public required string OriginalUrl { get; init; }

    [Key(2)]
    public required string CdnUrl { get; init; }

    [Key(3)]
    public required int Width { get; init; }

    [Key(4)]
    public required int Height { get; init; }

    [Key(5)]
    public required long FileSizeBytes { get; init; }

    [Key(6)]
    public required string ContentType { get; init; }

    [Key(7)]
    public string? BlurHash { get; init; }

    [Key(8)]
    public string? DominantColor { get; init; }
}
