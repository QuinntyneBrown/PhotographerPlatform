using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Core.Repositories;

public interface IWatermarkRepository
{
    Task<Watermark?> GetByIdAsync(string watermarkId, CancellationToken ct = default);
    Task<IReadOnlyList<Watermark>> ListByAccountAsync(string accountId, CancellationToken ct = default);
    Task AddAsync(Watermark watermark, CancellationToken ct = default);
    Task DeleteAsync(string watermarkId, CancellationToken ct = default);
}
