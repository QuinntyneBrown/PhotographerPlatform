using PhotographerPlatform.Communication.Core.Models;

namespace PhotographerPlatform.Communication.Core.Services;

public interface INotificationService
{
    Task<EmailLog> SendAsync(ManualNotificationRequest request, CancellationToken ct = default);
    Task<RenderedTemplate> PreviewAsync(PreviewRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<EmailTemplate>> ListTemplatesAsync(CancellationToken ct = default);
    Task<EmailTemplate> UpdateTemplateAsync(string templateId, string subject, string body, List<string> variables, CancellationToken ct = default);
    Task<IReadOnlyList<EmailLog>> ListLogsAsync(int? take, CancellationToken ct = default);
}

public sealed class ManualNotificationRequest
{
    public required string Recipient { get; init; }
    public EmailTemplateType? TemplateType { get; init; }
    public string? TemplateId { get; init; }
    public Dictionary<string, string> Variables { get; init; } = new();
    public Dictionary<string, string> Metadata { get; init; } = new();
}

public sealed class PreviewRequest
{
    public EmailTemplateType? TemplateType { get; init; }
    public string? TemplateId { get; init; }
    public Dictionary<string, string> Variables { get; init; } = new();
}

public interface IMessagingService
{
    Task<IReadOnlyList<Message>> ListMessagesAsync(string projectId, CancellationToken ct = default);
    Task<Message> AddMessageAsync(string projectId, string senderId, string content, MessageVisibility visibility, string? threadId, CancellationToken ct = default);
    Task<Message> UpdateMessageAsync(string messageId, string content, CancellationToken ct = default);
    Task<Message> SetVisibilityAsync(string messageId, MessageVisibility visibility, CancellationToken ct = default);
    Task DeleteMessageAsync(string messageId, CancellationToken ct = default);

    Task<IReadOnlyList<Note>> ListNotesAsync(string projectId, CancellationToken ct = default);
    Task<Note> AddNoteAsync(string projectId, string authorId, string content, NoteCategory category, bool internalOnly, CancellationToken ct = default);
    Task<Note> UpdateNoteAsync(string noteId, string content, NoteCategory category, bool internalOnly, CancellationToken ct = default);
    Task DeleteNoteAsync(string noteId, CancellationToken ct = default);
}
