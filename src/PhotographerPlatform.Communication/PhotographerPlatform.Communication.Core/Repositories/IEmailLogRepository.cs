using PhotographerPlatform.Communication.Core.Models;

namespace PhotographerPlatform.Communication.Core.Repositories;

public interface IEmailLogRepository
{
    Task AddAsync(EmailLog log, CancellationToken ct = default);
    Task<IReadOnlyList<EmailLog>> ListAsync(int? take = null, CancellationToken ct = default);
}
