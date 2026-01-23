using PhotographerPlatform.Galleries.Core.Services;

namespace PhotographerPlatform.Galleries.Infrastructure.Storage;

public sealed class FileSystemStorageService : IStorageService
{
    private readonly string _rootPath;

    public FileSystemStorageService(string rootPath)
    {
        _rootPath = rootPath;
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<StoredFile> SaveAsync(Stream content, string fileName, string container, string contentType, CancellationToken ct = default)
    {
        var safeName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
        var folder = Path.Combine(_rootPath, container);
        Directory.CreateDirectory(folder);
        var storedName = $"{Guid.NewGuid():N}_{safeName}";
        var path = Path.Combine(folder, storedName);

        await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);
        await content.CopyToAsync(fileStream, ct).ConfigureAwait(false);

        return new StoredFile
        {
            Path = path,
            FileName = storedName,
            ContentType = contentType,
            Length = fileStream.Length
        };
    }

    public Task<Stream> OpenReadAsync(string path, CancellationToken ct = default)
    {
        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
        return Task.FromResult(stream);
    }

    public Task DeleteAsync(string path, CancellationToken ct = default)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string path, CancellationToken ct = default)
    {
        return Task.FromResult(File.Exists(path));
    }
}
