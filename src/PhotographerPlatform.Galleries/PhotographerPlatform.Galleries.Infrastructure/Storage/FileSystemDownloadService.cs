using System.IO.Compression;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Services;

namespace PhotographerPlatform.Galleries.Infrastructure.Storage;

public sealed class FileSystemDownloadService : IDownloadService
{
    private readonly IStorageService _storage;

    public FileSystemDownloadService(IStorageService storage)
    {
        _storage = storage;
    }

    public async Task<DownloadResult> GetImageDownloadAsync(Image image, Gallery gallery, DownloadPermission permission, CancellationToken ct = default)
    {
        var path = permission switch
        {
            DownloadPermission.WebSize => !string.IsNullOrWhiteSpace(image.WebPath) ? image.WebPath : image.OriginalPath,
            DownloadPermission.FullSize => image.OriginalPath,
            _ => throw new InvalidOperationException("Downloads are not permitted.")
        };

        var stream = await _storage.OpenReadAsync(path, ct).ConfigureAwait(false);
        return new DownloadResult
        {
            Content = stream,
            FileName = image.FileName,
            ContentType = "application/octet-stream"
        };
    }

    public async Task<DownloadResult> CreateGalleryZipAsync(Gallery gallery, IReadOnlyList<Image> images, DownloadPermission permission, CancellationToken ct = default)
    {
        var memory = new MemoryStream();
        using (var archive = new ZipArchive(memory, ZipArchiveMode.Create, true))
        {
            foreach (var image in images)
            {
                var path = permission == DownloadPermission.WebSize && !string.IsNullOrWhiteSpace(image.WebPath)
                    ? image.WebPath
                    : image.OriginalPath;

                if (!await _storage.ExistsAsync(path, ct).ConfigureAwait(false))
                {
                    continue;
                }

                await using var content = await _storage.OpenReadAsync(path, ct).ConfigureAwait(false);
                var entry = archive.CreateEntry(image.FileName, CompressionLevel.Fastest);
                await using var entryStream = entry.Open();
                await content.CopyToAsync(entryStream, ct).ConfigureAwait(false);
            }
        }

        memory.Position = 0;
        return new DownloadResult
        {
            Content = memory,
            FileName = $"gallery_{gallery.GalleryId}.zip",
            ContentType = "application/zip"
        };
    }
}
