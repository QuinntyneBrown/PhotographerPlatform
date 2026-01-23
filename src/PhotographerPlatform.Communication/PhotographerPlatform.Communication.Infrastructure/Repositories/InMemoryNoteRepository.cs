using System.Collections.Concurrent;
using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Repositories;

namespace PhotographerPlatform.Communication.Infrastructure.Repositories;

public sealed class InMemoryNoteRepository : INoteRepository
{
    private readonly ConcurrentDictionary<string, Note> _notes = new();

    public Task<Note?> GetByIdAsync(string noteId, CancellationToken ct = default)
    {
        _notes.TryGetValue(noteId, out var note);
        return Task.FromResult(note);
    }

    public Task<IReadOnlyList<Note>> ListByProjectAsync(string projectId, CancellationToken ct = default)
    {
        var results = _notes.Values.Where(note => note.ProjectId == projectId)
            .OrderBy(note => note.CreatedAtUnixMs)
            .ToList();
        return Task.FromResult<IReadOnlyList<Note>>(results);
    }

    public Task AddAsync(Note note, CancellationToken ct = default)
    {
        _notes[note.NoteId] = note;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Note note, CancellationToken ct = default)
    {
        _notes[note.NoteId] = note;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string noteId, CancellationToken ct = default)
    {
        _notes.TryRemove(noteId, out _);
        return Task.CompletedTask;
    }
}
