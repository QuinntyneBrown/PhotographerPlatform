namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class WatermarkSettings
{
    public bool Enabled { get; set; }
    public string? WatermarkId { get; set; }
    public WatermarkPosition Position { get; set; } = WatermarkPosition.BottomRight;
    public double Opacity { get; set; } = 0.6;
    public double Scale { get; set; } = 0.2;
}
