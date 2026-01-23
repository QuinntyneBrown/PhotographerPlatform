namespace PhotographerPlatform.Communication.Core.Models;

public sealed class EmailTemplate
{
    public required string TemplateId { get; init; }
    public required EmailTemplateType Type { get; init; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public List<string> Variables { get; set; } = new();
    public int Version { get; set; } = 1;
    public long CreatedAtUnixMs { get; init; }
    public long UpdatedAtUnixMs { get; set; }
}
