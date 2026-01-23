using PhotographerPlatform.Workspace.Core.Models;

namespace PhotographerPlatform.Workspace.Core.Repositories;

public interface ITagRepository
{
    Task<IReadOnlyList<Tag>> ListAsync(string accountId, CancellationToken ct = default);
    Task AddAsync(Tag tag, CancellationToken ct = default);
}
