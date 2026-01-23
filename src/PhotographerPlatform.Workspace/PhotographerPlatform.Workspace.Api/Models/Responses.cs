using PhotographerPlatform.Workspace.Core.Models;

namespace PhotographerPlatform.Workspace.Api.Models;

public sealed class SearchResponse
{
    public IReadOnlyList<Project> Projects { get; init; } = Array.Empty<Project>();
    public IReadOnlyList<Client> Clients { get; init; } = Array.Empty<Client>();
}
