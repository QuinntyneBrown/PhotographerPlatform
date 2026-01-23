using System.Collections.Concurrent;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Repositories;

namespace PhotographerPlatform.Galleries.Infrastructure.Repositories;

public sealed class InMemoryFavoriteRepository : IFavoriteRepository
{
    private readonly ConcurrentDictionary<string, Favorite> _favorites = new();

    public Task<Favorite?> GetAsync(string galleryId, string imageId, string clientId, CancellationToken ct = default)
    {
        var favorite = _favorites.Values.FirstOrDefault(item => item.GalleryId == galleryId && item.ImageId == imageId && item.ClientId == clientId);
        return Task.FromResult(favorite);
    }

    public Task<IReadOnlyList<Favorite>> ListByGalleryAsync(string galleryId, string clientId, CancellationToken ct = default)
    {
        var results = _favorites.Values
            .Where(item => item.GalleryId == galleryId && item.ClientId == clientId)
            .ToList();
        return Task.FromResult<IReadOnlyList<Favorite>>(results);
    }

    public Task AddAsync(Favorite favorite, CancellationToken ct = default)
    {
        _favorites[favorite.FavoriteId] = favorite;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string favoriteId, CancellationToken ct = default)
    {
        _favorites.TryRemove(favoriteId, out _);
        return Task.CompletedTask;
    }

    public Task<int> CountByImageAsync(string imageId, CancellationToken ct = default)
    {
        var count = _favorites.Values.Count(item => item.ImageId == imageId);
        return Task.FromResult(count);
    }
}
