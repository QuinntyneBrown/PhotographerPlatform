using System.Collections.Concurrent;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Repositories;

namespace PhotographerPlatform.Galleries.Infrastructure.Repositories;

public sealed class InMemoryGalleryRepository : IGalleryRepository
{
    private readonly ConcurrentDictionary<string, Gallery> _galleries = new();

    public Task<Gallery?> GetByIdAsync(string galleryId, CancellationToken ct = default)
    {
        _galleries.TryGetValue(galleryId, out var gallery);
        return Task.FromResult(gallery);
    }

    public Task<IReadOnlyList<Gallery>> ListByProjectAsync(string projectId, CancellationToken ct = default)
    {
        var results = _galleries.Values.Where(gallery => gallery.ProjectId == projectId).ToList();
        return Task.FromResult<IReadOnlyList<Gallery>>(results);
    }

    public Task AddAsync(Gallery gallery, CancellationToken ct = default)
    {
        _galleries[gallery.GalleryId] = gallery;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Gallery gallery, CancellationToken ct = default)
    {
        _galleries[gallery.GalleryId] = gallery;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string galleryId, CancellationToken ct = default)
    {
        _galleries.TryRemove(galleryId, out _);
        return Task.CompletedTask;
    }
}
