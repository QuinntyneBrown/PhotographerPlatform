using MessagePack;

namespace Shared.Cdn;

[MessagePackObject]
public sealed class ResponsiveImageVariant
{
    [Key(0)]
    public required string Url { get; init; }

    [Key(1)]
    public required int Width { get; init; }

    [Key(2)]
    public int? Height { get; init; }

    [Key(3)]
    public required ImageFormat Format { get; init; }

    [Key(4)]
    public double DevicePixelRatio { get; init; } = 1.0;
}
