using System.Collections.Concurrent;
using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Repositories;

namespace PhotographerPlatform.Communication.Infrastructure.Repositories;

public sealed class InMemoryEmailLogRepository : IEmailLogRepository
{
    private readonly ConcurrentDictionary<string, EmailLog> _logs = new();

    public Task AddAsync(EmailLog log, CancellationToken ct = default)
    {
        _logs[log.EmailLogId] = log;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<EmailLog>> ListAsync(int? take = null, CancellationToken ct = default)
    {
        var ordered = _logs.Values.OrderByDescending(log => log.CreatedAtUnixMs);
        if (take.HasValue)
        {
            ordered = ordered.Take(take.Value);
        }

        return Task.FromResult<IReadOnlyList<EmailLog>>(ordered.ToList());
    }
}
