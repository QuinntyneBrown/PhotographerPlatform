namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class DownloadSettings
{
    public DownloadPermission Permission { get; set; } = DownloadPermission.None;
    public long? ExpiresAtUnixMs { get; set; }
    public int WebSizeMaxWidth { get; set; } = 2048;
    public int WebSizeMaxHeight { get; set; } = 2048;
    public int WebSizeQuality { get; set; } = 85;
}
