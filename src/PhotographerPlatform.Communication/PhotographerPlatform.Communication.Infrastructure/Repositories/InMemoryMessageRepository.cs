using System.Collections.Concurrent;
using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Repositories;

namespace PhotographerPlatform.Communication.Infrastructure.Repositories;

public sealed class InMemoryMessageRepository : IMessageRepository
{
    private readonly ConcurrentDictionary<string, Message> _messages = new();

    public Task<Message?> GetByIdAsync(string messageId, CancellationToken ct = default)
    {
        _messages.TryGetValue(messageId, out var message);
        return Task.FromResult(message);
    }

    public Task<IReadOnlyList<Message>> ListByProjectAsync(string projectId, CancellationToken ct = default)
    {
        var results = _messages.Values.Where(message => message.ProjectId == projectId)
            .OrderBy(message => message.CreatedAtUnixMs)
            .ToList();
        return Task.FromResult<IReadOnlyList<Message>>(results);
    }

    public Task AddAsync(Message message, CancellationToken ct = default)
    {
        _messages[message.MessageId] = message;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Message message, CancellationToken ct = default)
    {
        _messages[message.MessageId] = message;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string messageId, CancellationToken ct = default)
    {
        _messages.TryRemove(messageId, out _);
        return Task.CompletedTask;
    }
}
