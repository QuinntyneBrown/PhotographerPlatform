using PhotographerPlatform.Communication.Core.Models;

namespace PhotographerPlatform.Communication.Core.Services;

public interface IEmailSender
{
    Task<EmailSendResult> SendAsync(EmailSendRequest request, CancellationToken ct = default);
}

public sealed class EmailSendRequest
{
    public required string Recipient { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
    public EmailTemplateType TemplateType { get; init; }
    public string? TemplateId { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = new();
}

public sealed class EmailSendResult
{
    public required bool Success { get; init; }
    public string? Error { get; init; }
}

public interface ITemplateRenderer
{
    RenderedTemplate Render(EmailTemplate template, IReadOnlyDictionary<string, string> variables);
}

public sealed class RenderedTemplate
{
    public required string Subject { get; init; }
    public required string Body { get; init; }
}
