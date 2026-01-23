using PhotographerPlatform.Workspace.Core.Models;
using PhotographerPlatform.Workspace.Core.Services;
using PhotographerPlatform.Workspace.Infrastructure.Repositories;
using Xunit;

namespace PhotographerPlatform.Workspace.Core.Tests;

public sealed class WorkspaceServiceTests
{
    private static WorkspaceService CreateService()
    {
        return new WorkspaceService(
            new InMemoryProjectRepository(),
            new InMemoryClientRepository(),
            new InMemoryClientNoteRepository(),
            new InMemoryTagRepository());
    }

    [Fact]
    public async Task CreateProject_PersistsTags()
    {
        var service = CreateService();
        var project = await service.CreateProjectAsync("acct", "Wedding", null, "Austin", new[] { "wedding", "summer" }, "user");

        Assert.Equal(2, project.Tags.Count);
        Assert.Contains("wedding", project.Tags, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ListProjects_FiltersByTag()
    {
        var service = CreateService();
        await service.CreateProjectAsync("acct", "One", null, null, new[] { "family" }, "user");
        await service.CreateProjectAsync("acct", "Two", null, null, new[] { "wedding" }, "user");

        var results = await service.ListProjectsAsync("acct", new ProjectQuery { Tags = new[] { "wedding" } });

        Assert.Single(results);
        Assert.Equal("Two", results[0].Name);
    }

    [Fact]
    public async Task AddClientNote_RequiresClient()
    {
        var service = CreateService();
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddClientNoteAsync("acct", "missing", "note", "user"));
    }

    [Fact]
    public async Task Search_ReturnsProjectsAndClients()
    {
        var service = CreateService();
        await service.CreateProjectAsync("acct", "Studio Session", null, "LA", new[] { "studio" }, "user");
        await service.CreateClientAsync("acct", "Alex Johnson", "alex@example.com", null, null, "user");

        var result = await service.SearchAsync("acct", "Alex");

        Assert.Single(result.Clients);
        Assert.Empty(result.Projects);
    }
}
