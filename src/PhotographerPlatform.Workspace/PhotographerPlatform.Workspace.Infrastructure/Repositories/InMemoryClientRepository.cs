using System.Collections.Concurrent;
using PhotographerPlatform.Workspace.Core.Models;
using PhotographerPlatform.Workspace.Core.Repositories;

namespace PhotographerPlatform.Workspace.Infrastructure.Repositories;

public sealed class InMemoryClientRepository : IClientRepository
{
    private readonly ConcurrentDictionary<string, Client> _clients = new();

    public Task<Client?> GetByIdAsync(string accountId, string clientId, CancellationToken ct = default)
    {
        if (_clients.TryGetValue(clientId, out var client) && client.AccountId == accountId)
        {
            return Task.FromResult<Client?>(client);
        }

        return Task.FromResult<Client?>(null);
    }

    public Task<IReadOnlyList<Client>> ListAsync(string accountId, CancellationToken ct = default)
    {
        var results = _clients.Values.Where(client => client.AccountId == accountId).ToList();
        return Task.FromResult<IReadOnlyList<Client>>(results);
    }

    public Task AddAsync(Client client, CancellationToken ct = default)
    {
        _clients[client.ClientId] = client;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Client client, CancellationToken ct = default)
    {
        _clients[client.ClientId] = client;
        return Task.CompletedTask;
    }
}
