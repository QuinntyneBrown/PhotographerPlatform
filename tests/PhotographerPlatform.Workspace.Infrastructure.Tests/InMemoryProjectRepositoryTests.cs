using PhotographerPlatform.Workspace.Core.Models;
using PhotographerPlatform.Workspace.Infrastructure.Repositories;
using Xunit;

namespace PhotographerPlatform.Workspace.Infrastructure.Tests;

public sealed class InMemoryProjectRepositoryTests
{
    [Fact]
    public async Task GetById_RespectsAccountScope()
    {
        var repo = new InMemoryProjectRepository();
        var project = new Project
        {
            ProjectId = "proj_1",
            AccountId = "acct_1",
            Name = "Session",
            Audit = new AuditFields { CreatedBy = "user", CreatedAtUnixMs = 1 }
        };

        await repo.AddAsync(project);

        var found = await repo.GetByIdAsync("acct_2", "proj_1");
        Assert.Null(found);
    }
}
