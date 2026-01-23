using System.Collections.Concurrent;
using PhotographerPlatform.Workspace.Core.Models;
using PhotographerPlatform.Workspace.Core.Repositories;

namespace PhotographerPlatform.Workspace.Infrastructure.Repositories;

public sealed class InMemoryClientNoteRepository : IClientNoteRepository
{
    private readonly ConcurrentDictionary<string, ClientNote> _notes = new();

    public Task<IReadOnlyList<ClientNote>> ListByClientAsync(string accountId, string clientId, CancellationToken ct = default)
    {
        var results = _notes.Values
            .Where(note => note.AccountId == accountId && note.ClientId == clientId)
            .OrderBy(note => note.CreatedAtUnixMs)
            .ToList();
        return Task.FromResult<IReadOnlyList<ClientNote>>(results);
    }

    public Task AddAsync(ClientNote note, CancellationToken ct = default)
    {
        _notes[note.NoteId] = note;
        return Task.CompletedTask;
    }
}
