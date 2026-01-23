using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Infrastructure.Repositories;
using Xunit;

namespace PhotographerPlatform.Communication.Infrastructure.Tests;

public sealed class InMemoryTemplateRepositoryTests
{
    [Fact]
    public async Task SeededTemplates_AreAvailableByType()
    {
        var repo = new InMemoryEmailTemplateRepository();
        var template = await repo.GetByTypeAsync(EmailTemplateType.Welcome);

        Assert.NotNull(template);
        Assert.Equal(EmailTemplateType.Welcome, template?.Type);
    }
}
