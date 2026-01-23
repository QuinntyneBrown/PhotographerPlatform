namespace PhotographerPlatform.Workspace.Api.Models;

public sealed class ProjectCreateRequest
{
    public required string Name { get; init; }
    public DateTimeOffset? EventDate { get; init; }
    public string? Location { get; init; }
    public List<string> Tags { get; init; } = new();
    public List<ProjectClientRequest> Clients { get; init; } = new();
}

public sealed class ProjectUpdateRequest
{
    public required string Name { get; init; }
    public DateTimeOffset? EventDate { get; init; }
    public string? Location { get; init; }
    public List<string> Tags { get; init; } = new();
    public List<ProjectClientRequest> Clients { get; init; } = new();
}

public sealed class ProjectClientRequest
{
    public required string ClientId { get; init; }
    public bool IsPrimary { get; init; }
}

public sealed class ClientCreateRequest
{
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
}

public sealed class ClientUpdateRequest
{
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
}

public sealed class ClientNoteRequest
{
    public required string Content { get; init; }
}
