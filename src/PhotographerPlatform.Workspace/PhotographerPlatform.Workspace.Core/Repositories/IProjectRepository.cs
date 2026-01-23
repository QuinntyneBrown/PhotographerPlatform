using PhotographerPlatform.Workspace.Core.Models;

namespace PhotographerPlatform.Workspace.Core.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(string accountId, string projectId, CancellationToken ct = default);
    Task<IReadOnlyList<Project>> ListAsync(string accountId, CancellationToken ct = default);
    Task AddAsync(Project project, CancellationToken ct = default);
    Task UpdateAsync(Project project, CancellationToken ct = default);
}
