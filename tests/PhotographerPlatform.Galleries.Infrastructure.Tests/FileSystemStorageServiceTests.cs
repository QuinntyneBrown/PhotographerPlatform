using System.Text;
using PhotographerPlatform.Galleries.Infrastructure.Storage;
using Xunit;

namespace PhotographerPlatform.Galleries.Infrastructure.Tests;

public sealed class FileSystemStorageServiceTests
{
    [Fact]
    public async Task SaveAsync_WritesAndReadsContent()
    {
        var root = Path.Combine(Path.GetTempPath(), "galleries-tests", Guid.NewGuid().ToString("N"));
        var storage = new FileSystemStorageService(root);

        await using var input = new MemoryStream(Encoding.UTF8.GetBytes("hello"));
        var stored = await storage.SaveAsync(input, "test.txt", "originals", "text/plain");

        Assert.True(File.Exists(stored.Path));

        await using var readStream = await storage.OpenReadAsync(stored.Path);
        using var reader = new StreamReader(readStream);
        var content = await reader.ReadToEndAsync();

        Assert.Equal("hello", content);
    }
}
