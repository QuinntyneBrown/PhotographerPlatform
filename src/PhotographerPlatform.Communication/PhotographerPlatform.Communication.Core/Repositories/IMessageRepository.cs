using PhotographerPlatform.Communication.Core.Models;

namespace PhotographerPlatform.Communication.Core.Repositories;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(string messageId, CancellationToken ct = default);
    Task<IReadOnlyList<Message>> ListByProjectAsync(string projectId, CancellationToken ct = default);
    Task AddAsync(Message message, CancellationToken ct = default);
    Task UpdateAsync(Message message, CancellationToken ct = default);
    Task DeleteAsync(string messageId, CancellationToken ct = default);
}
