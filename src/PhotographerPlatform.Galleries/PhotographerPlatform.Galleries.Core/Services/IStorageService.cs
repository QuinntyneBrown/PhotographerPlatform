namespace PhotographerPlatform.Galleries.Core.Services;

public sealed class StoredFile
{
    public required string Path { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public long Length { get; init; }
}

public interface IStorageService
{
    Task<StoredFile> SaveAsync(Stream content, string fileName, string container, string contentType, CancellationToken ct = default);
    Task<Stream> OpenReadAsync(string path, CancellationToken ct = default);
    Task DeleteAsync(string path, CancellationToken ct = default);
    Task<bool> ExistsAsync(string path, CancellationToken ct = default);
}
