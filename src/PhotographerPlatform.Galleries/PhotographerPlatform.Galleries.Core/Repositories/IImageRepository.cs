using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Core.Repositories;

public interface IImageRepository
{
    Task<Image?> GetByIdAsync(string imageId, CancellationToken ct = default);
    Task<IReadOnlyList<Image>> ListByGalleryAsync(string galleryId, CancellationToken ct = default);
    Task AddRangeAsync(IReadOnlyList<Image> images, CancellationToken ct = default);
    Task UpdateAsync(Image image, CancellationToken ct = default);
    Task UpdateRangeAsync(IReadOnlyList<Image> images, CancellationToken ct = default);
    Task DeleteAsync(string imageId, CancellationToken ct = default);
}
