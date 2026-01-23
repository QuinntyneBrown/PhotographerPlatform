namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class GallerySettings
{
    public GalleryAccess Access { get; set; } = new();
    public DownloadSettings Downloads { get; set; } = new();
    public ProofingSettings Proofing { get; set; } = new();
    public WatermarkSettings Watermark { get; set; } = new();
}
