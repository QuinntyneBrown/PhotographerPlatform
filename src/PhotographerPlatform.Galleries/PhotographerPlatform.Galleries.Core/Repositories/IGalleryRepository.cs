using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Core.Repositories;

public interface IGalleryRepository
{
    Task<Gallery?> GetByIdAsync(string galleryId, CancellationToken ct = default);
    Task<IReadOnlyList<Gallery>> ListByProjectAsync(string projectId, CancellationToken ct = default);
    Task AddAsync(Gallery gallery, CancellationToken ct = default);
    Task UpdateAsync(Gallery gallery, CancellationToken ct = default);
    Task DeleteAsync(string galleryId, CancellationToken ct = default);
}
