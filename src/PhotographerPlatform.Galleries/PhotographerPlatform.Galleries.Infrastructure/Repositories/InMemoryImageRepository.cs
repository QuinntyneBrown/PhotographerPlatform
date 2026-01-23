using System.Collections.Concurrent;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Repositories;

namespace PhotographerPlatform.Galleries.Infrastructure.Repositories;

public sealed class InMemoryImageRepository : IImageRepository
{
    private readonly ConcurrentDictionary<string, Image> _images = new();

    public Task<Image?> GetByIdAsync(string imageId, CancellationToken ct = default)
    {
        _images.TryGetValue(imageId, out var image);
        return Task.FromResult(image);
    }

    public Task<IReadOnlyList<Image>> ListByGalleryAsync(string galleryId, CancellationToken ct = default)
    {
        var results = _images.Values.Where(image => image.GalleryId == galleryId)
            .OrderBy(image => image.Order)
            .ToList();
        return Task.FromResult<IReadOnlyList<Image>>(results);
    }

    public Task AddRangeAsync(IReadOnlyList<Image> images, CancellationToken ct = default)
    {
        foreach (var image in images)
        {
            _images[image.ImageId] = image;
        }

        return Task.CompletedTask;
    }

    public Task UpdateAsync(Image image, CancellationToken ct = default)
    {
        _images[image.ImageId] = image;
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IReadOnlyList<Image> images, CancellationToken ct = default)
    {
        foreach (var image in images)
        {
            _images[image.ImageId] = image;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(string imageId, CancellationToken ct = default)
    {
        _images.TryRemove(imageId, out _);
        return Task.CompletedTask;
    }
}
