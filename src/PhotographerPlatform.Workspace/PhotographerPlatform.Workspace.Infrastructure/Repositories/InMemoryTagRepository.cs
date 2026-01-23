using System.Collections.Concurrent;
using PhotographerPlatform.Workspace.Core.Models;
using PhotographerPlatform.Workspace.Core.Repositories;

namespace PhotographerPlatform.Workspace.Infrastructure.Repositories;

public sealed class InMemoryTagRepository : ITagRepository
{
    private readonly ConcurrentDictionary<string, Tag> _tags = new();

    public Task<IReadOnlyList<Tag>> ListAsync(string accountId, CancellationToken ct = default)
    {
        var results = _tags.Values.Where(tag => tag.AccountId == accountId).ToList();
        return Task.FromResult<IReadOnlyList<Tag>>(results);
    }

    public Task AddAsync(Tag tag, CancellationToken ct = default)
    {
        _tags[tag.TagId] = tag;
        return Task.CompletedTask;
    }
}
