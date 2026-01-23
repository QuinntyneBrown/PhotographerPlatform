namespace PhotographerPlatform.Galleries.Core.Models;

public sealed class ProofingSettings
{
    public bool Enabled { get; set; } = true;
    public int? SelectionLimit { get; set; }
    public long? DeadlineUnixMs { get; set; }
}
