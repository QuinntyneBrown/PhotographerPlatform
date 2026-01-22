using MessagePack;

namespace Shared.Cdn;

[MessagePackObject]
public sealed class ImageTransformOptions
{
    [Key(0)]
    public int? Width { get; init; }

    [Key(1)]
    public int? Height { get; init; }

    [Key(2)]
    public ImageFormat Format { get; init; } = ImageFormat.Original;

    [Key(3)]
    public int Quality { get; init; } = 85;

    [Key(4)]
    public ImageFit Fit { get; init; } = ImageFit.Cover;

    [Key(5)]
    public bool ApplyWatermark { get; init; }

    [Key(6)]
    public string? WatermarkId { get; init; }

    [Key(7)]
    public double? DevicePixelRatio { get; init; }
}
