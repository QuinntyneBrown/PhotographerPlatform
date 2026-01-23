using PhotographerPlatform.Communication.Core.Models;

namespace PhotographerPlatform.Communication.Core.Repositories;

public interface INoteRepository
{
    Task<Note?> GetByIdAsync(string noteId, CancellationToken ct = default);
    Task<IReadOnlyList<Note>> ListByProjectAsync(string projectId, CancellationToken ct = default);
    Task AddAsync(Note note, CancellationToken ct = default);
    Task UpdateAsync(Note note, CancellationToken ct = default);
    Task DeleteAsync(string noteId, CancellationToken ct = default);
}
