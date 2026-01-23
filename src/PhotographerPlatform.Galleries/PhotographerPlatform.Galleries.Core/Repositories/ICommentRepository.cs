using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Core.Repositories;

public interface ICommentRepository
{
    Task<IReadOnlyList<Comment>> ListByImageAsync(string imageId, CancellationToken ct = default);
    Task AddAsync(Comment comment, CancellationToken ct = default);
    Task DeleteAsync(string commentId, CancellationToken ct = default);
}
