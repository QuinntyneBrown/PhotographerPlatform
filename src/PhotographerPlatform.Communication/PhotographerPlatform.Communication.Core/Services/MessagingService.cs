using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Repositories;

namespace PhotographerPlatform.Communication.Core.Services;

public sealed class MessagingService : IMessagingService
{
    private readonly IMessageRepository _messages;
    private readonly INoteRepository _notes;

    public MessagingService(IMessageRepository messages, INoteRepository notes)
    {
        _messages = messages;
        _notes = notes;
    }

    public Task<IReadOnlyList<Message>> ListMessagesAsync(string projectId, CancellationToken ct = default)
    {
        return _messages.ListByProjectAsync(projectId, ct);
    }

    public async Task<Message> AddMessageAsync(string projectId, string senderId, string content, MessageVisibility visibility, string? threadId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content is required.", nameof(content));
        }

        var message = new Message
        {
            MessageId = Guid.NewGuid().ToString("N"),
            ProjectId = projectId,
            SenderId = senderId,
            Content = content,
            Visibility = visibility,
            ThreadId = threadId,
            CreatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        await _messages.AddAsync(message, ct).ConfigureAwait(false);
        return message;
    }

    public async Task<Message> UpdateMessageAsync(string messageId, string content, CancellationToken ct = default)
    {
        var message = await RequireMessageAsync(messageId, ct).ConfigureAwait(false);
        message.Content = content;
        message.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _messages.UpdateAsync(message, ct).ConfigureAwait(false);
        return message;
    }

    public async Task<Message> SetVisibilityAsync(string messageId, MessageVisibility visibility, CancellationToken ct = default)
    {
        var message = await RequireMessageAsync(messageId, ct).ConfigureAwait(false);
        message.Visibility = visibility;
        message.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _messages.UpdateAsync(message, ct).ConfigureAwait(false);
        return message;
    }

    public Task DeleteMessageAsync(string messageId, CancellationToken ct = default)
    {
        return _messages.DeleteAsync(messageId, ct);
    }

    public Task<IReadOnlyList<Note>> ListNotesAsync(string projectId, CancellationToken ct = default)
    {
        return _notes.ListByProjectAsync(projectId, ct);
    }

    public async Task<Note> AddNoteAsync(string projectId, string authorId, string content, NoteCategory category, bool internalOnly, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content is required.", nameof(content));
        }

        var note = new Note
        {
            NoteId = Guid.NewGuid().ToString("N"),
            ProjectId = projectId,
            AuthorId = authorId,
            Content = content,
            Category = category,
            InternalOnly = internalOnly,
            CreatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        await _notes.AddAsync(note, ct).ConfigureAwait(false);
        return note;
    }

    public async Task<Note> UpdateNoteAsync(string noteId, string content, NoteCategory category, bool internalOnly, CancellationToken ct = default)
    {
        var note = await RequireNoteAsync(noteId, ct).ConfigureAwait(false);
        note.Content = content;
        note.Category = category;
        note.InternalOnly = internalOnly;
        note.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _notes.UpdateAsync(note, ct).ConfigureAwait(false);
        return note;
    }

    public Task DeleteNoteAsync(string noteId, CancellationToken ct = default)
    {
        return _notes.DeleteAsync(noteId, ct);
    }

    private async Task<Message> RequireMessageAsync(string messageId, CancellationToken ct)
    {
        var message = await _messages.GetByIdAsync(messageId, ct).ConfigureAwait(false);
        if (message is null)
        {
            throw new InvalidOperationException("Message not found.");
        }

        return message;
    }

    private async Task<Note> RequireNoteAsync(string noteId, CancellationToken ct)
    {
        var note = await _notes.GetByIdAsync(noteId, ct).ConfigureAwait(false);
        if (note is null)
        {
            throw new InvalidOperationException("Note not found.");
        }

        return note;
    }
}
