using PhotographerPlatform.Workspace.Core.Models;

namespace PhotographerPlatform.Workspace.Core.Repositories;

public interface IClientNoteRepository
{
    Task<IReadOnlyList<ClientNote>> ListByClientAsync(string accountId, string clientId, CancellationToken ct = default);
    Task AddAsync(ClientNote note, CancellationToken ct = default);
}
