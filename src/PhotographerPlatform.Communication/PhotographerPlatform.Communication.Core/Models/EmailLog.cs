namespace PhotographerPlatform.Communication.Core.Models;

public sealed class EmailLog
{
    public required string EmailLogId { get; init; }
    public required string Recipient { get; init; }
    public required EmailTemplateType TemplateType { get; init; }
    public string? TemplateId { get; init; }
    public required EmailStatus Status { get; set; }
    public long CreatedAtUnixMs { get; init; }
    public long? SentAtUnixMs { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, string> Metadata { get; init; } = new();
}
