namespace PhotographerPlatform.Workspace.Core.Models;

public sealed class AuditFields
{
    public required string CreatedBy { get; init; }
    public required long CreatedAtUnixMs { get; init; }
    public string? UpdatedBy { get; set; }
    public long? UpdatedAtUnixMs { get; set; }
}
