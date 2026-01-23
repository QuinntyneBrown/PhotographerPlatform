namespace PhotographerPlatform.Workspace.Core.Models;

public sealed class ProjectClient
{
    public required string ClientId { get; init; }
    public bool IsPrimary { get; init; }
}
