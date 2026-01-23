using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Core.Repositories;

public interface IImageSetRepository
{
    Task<ImageSet?> GetByIdAsync(string setId, CancellationToken ct = default);
    Task<IReadOnlyList<ImageSet>> ListByGalleryAsync(string galleryId, CancellationToken ct = default);
    Task AddAsync(ImageSet imageSet, CancellationToken ct = default);
    Task UpdateAsync(ImageSet imageSet, CancellationToken ct = default);
    Task DeleteAsync(string setId, CancellationToken ct = default);
}
