using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Core.Repositories;

public interface IFavoriteRepository
{
    Task<Favorite?> GetAsync(string galleryId, string imageId, string clientId, CancellationToken ct = default);
    Task<IReadOnlyList<Favorite>> ListByGalleryAsync(string galleryId, string clientId, CancellationToken ct = default);
    Task AddAsync(Favorite favorite, CancellationToken ct = default);
    Task DeleteAsync(string favoriteId, CancellationToken ct = default);
    Task<int> CountByImageAsync(string imageId, CancellationToken ct = default);
}
