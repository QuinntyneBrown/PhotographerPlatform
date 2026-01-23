using PhotographerPlatform.Workspace.Core.Models;

namespace PhotographerPlatform.Workspace.Core.Repositories;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(string accountId, string clientId, CancellationToken ct = default);
    Task<IReadOnlyList<Client>> ListAsync(string accountId, CancellationToken ct = default);
    Task AddAsync(Client client, CancellationToken ct = default);
    Task UpdateAsync(Client client, CancellationToken ct = default);
}
