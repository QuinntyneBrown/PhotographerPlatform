using System.Collections.Concurrent;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Repositories;

namespace PhotographerPlatform.Galleries.Infrastructure.Repositories;

public sealed class InMemoryWatermarkRepository : IWatermarkRepository
{
    private readonly ConcurrentDictionary<string, Watermark> _watermarks = new();

    public Task<Watermark?> GetByIdAsync(string watermarkId, CancellationToken ct = default)
    {
        _watermarks.TryGetValue(watermarkId, out var watermark);
        return Task.FromResult(watermark);
    }

    public Task<IReadOnlyList<Watermark>> ListByAccountAsync(string accountId, CancellationToken ct = default)
    {
        var results = _watermarks.Values.Where(watermark => watermark.AccountId == accountId).ToList();
        return Task.FromResult<IReadOnlyList<Watermark>>(results);
    }

    public Task AddAsync(Watermark watermark, CancellationToken ct = default)
    {
        _watermarks[watermark.WatermarkId] = watermark;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string watermarkId, CancellationToken ct = default)
    {
        _watermarks.TryRemove(watermarkId, out _);
        return Task.CompletedTask;
    }
}
