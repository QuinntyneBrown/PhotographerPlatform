using System.Collections.Concurrent;
using PhotographerPlatform.Workspace.Core.Models;
using PhotographerPlatform.Workspace.Core.Repositories;

namespace PhotographerPlatform.Workspace.Infrastructure.Repositories;

public sealed class InMemoryProjectRepository : IProjectRepository
{
    private readonly ConcurrentDictionary<string, Project> _projects = new();

    public Task<Project?> GetByIdAsync(string accountId, string projectId, CancellationToken ct = default)
    {
        if (_projects.TryGetValue(projectId, out var project) && project.AccountId == accountId)
        {
            return Task.FromResult<Project?>(project);
        }

        return Task.FromResult<Project?>(null);
    }

    public Task<IReadOnlyList<Project>> ListAsync(string accountId, CancellationToken ct = default)
    {
        var results = _projects.Values.Where(project => project.AccountId == accountId).ToList();
        return Task.FromResult<IReadOnlyList<Project>>(results);
    }

    public Task AddAsync(Project project, CancellationToken ct = default)
    {
        _projects[project.ProjectId] = project;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Project project, CancellationToken ct = default)
    {
        _projects[project.ProjectId] = project;
        return Task.CompletedTask;
    }
}
