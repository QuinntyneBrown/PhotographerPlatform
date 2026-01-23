using System.Collections.Concurrent;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Repositories;

namespace PhotographerPlatform.Galleries.Infrastructure.Repositories;

public sealed class InMemoryImageSetRepository : IImageSetRepository
{
    private readonly ConcurrentDictionary<string, ImageSet> _sets = new();

    public Task<ImageSet?> GetByIdAsync(string setId, CancellationToken ct = default)
    {
        _sets.TryGetValue(setId, out var set);
        return Task.FromResult(set);
    }

    public Task<IReadOnlyList<ImageSet>> ListByGalleryAsync(string galleryId, CancellationToken ct = default)
    {
        var results = _sets.Values.Where(set => set.GalleryId == galleryId)
            .OrderBy(set => set.Order)
            .ToList();
        return Task.FromResult<IReadOnlyList<ImageSet>>(results);
    }

    public Task AddAsync(ImageSet imageSet, CancellationToken ct = default)
    {
        _sets[imageSet.ImageSetId] = imageSet;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ImageSet imageSet, CancellationToken ct = default)
    {
        _sets[imageSet.ImageSetId] = imageSet;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string setId, CancellationToken ct = default)
    {
        _sets.TryRemove(setId, out _);
        return Task.CompletedTask;
    }
}
