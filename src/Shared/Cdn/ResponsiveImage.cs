using MessagePack;

namespace Shared.Cdn;

[MessagePackObject]
public sealed class ResponsiveImage
{
    [Key(0)]
    public required string ImageId { get; init; }

    [Key(1)]
    public required string OriginalUrl { get; init; }

    [Key(2)]
    public required int OriginalWidth { get; init; }

    [Key(3)]
    public required int OriginalHeight { get; init; }

    [Key(4)]
    public required List<ResponsiveImageVariant> Variants { get; init; }

    [Key(5)]
    public string? BlurHash { get; init; }

    [Key(6)]
    public string? DominantColor { get; init; }
}
