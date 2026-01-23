using PhotographerPlatform.Communication.Core.Models;

namespace PhotographerPlatform.Communication.Api.Models;

public sealed class SendNotificationRequest
{
    public required string Recipient { get; init; }
    public EmailTemplateType? TemplateType { get; init; }
    public string? TemplateId { get; init; }
    public Dictionary<string, string> Variables { get; init; } = new();
    public Dictionary<string, string> Metadata { get; init; } = new();
}

public sealed class UpdateTemplateRequest
{
    public required string Subject { get; init; }
    public required string Body { get; init; }
    public List<string> Variables { get; init; } = new();
}

public sealed class PreviewTemplateRequest
{
    public EmailTemplateType? TemplateType { get; init; }
    public string? TemplateId { get; init; }
    public Dictionary<string, string> Variables { get; init; } = new();
}

public sealed class MessageCreateRequest
{
    public required string SenderId { get; init; }
    public required string Content { get; init; }
    public MessageVisibility Visibility { get; init; } = MessageVisibility.Internal;
    public string? ThreadId { get; init; }
}

public sealed class MessageUpdateRequest
{
    public required string Content { get; init; }
}

public sealed class MessageVisibilityRequest
{
    public required MessageVisibility Visibility { get; init; }
}

public sealed class NoteCreateRequest
{
    public required string AuthorId { get; init; }
    public required string Content { get; init; }
    public NoteCategory Category { get; init; } = NoteCategory.General;
    public bool InternalOnly { get; init; } = true;
}

public sealed class NoteUpdateRequest
{
    public required string Content { get; init; }
    public NoteCategory Category { get; init; } = NoteCategory.General;
    public bool InternalOnly { get; init; } = true;
}
