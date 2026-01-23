using System.Collections.Concurrent;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Repositories;

namespace PhotographerPlatform.Galleries.Infrastructure.Repositories;

public sealed class InMemoryCommentRepository : ICommentRepository
{
    private readonly ConcurrentDictionary<string, Comment> _comments = new();

    public Task<IReadOnlyList<Comment>> ListByImageAsync(string imageId, CancellationToken ct = default)
    {
        var results = _comments.Values
            .Where(comment => comment.ImageId == imageId)
            .OrderBy(comment => comment.CreatedAtUnixMs)
            .ToList();
        return Task.FromResult<IReadOnlyList<Comment>>(results);
    }

    public Task AddAsync(Comment comment, CancellationToken ct = default)
    {
        _comments[comment.CommentId] = comment;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string commentId, CancellationToken ct = default)
    {
        _comments.TryRemove(commentId, out _);
        return Task.CompletedTask;
    }
}
