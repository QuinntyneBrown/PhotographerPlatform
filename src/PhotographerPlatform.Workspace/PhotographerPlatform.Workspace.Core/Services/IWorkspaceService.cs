using PhotographerPlatform.Workspace.Core.Models;

namespace PhotographerPlatform.Workspace.Core.Services;

public interface IWorkspaceService
{
    Task<Project> CreateProjectAsync(string accountId, string name, DateTimeOffset? eventDate, string? location, IReadOnlyList<string>? tags, string createdBy, CancellationToken ct = default);
    Task<Project?> GetProjectAsync(string accountId, string projectId, CancellationToken ct = default);
    Task<Project> UpdateProjectAsync(string accountId, string projectId, string name, DateTimeOffset? eventDate, string? location, IReadOnlyList<string>? tags, string updatedBy, CancellationToken ct = default);
    Task<Project> SetProjectClientsAsync(string accountId, string projectId, IReadOnlyList<ProjectClient> clients, string updatedBy, CancellationToken ct = default);
    Task ArchiveProjectAsync(string accountId, string projectId, string updatedBy, CancellationToken ct = default);
    Task UnarchiveProjectAsync(string accountId, string projectId, string updatedBy, CancellationToken ct = default);
    Task<IReadOnlyList<Project>> ListProjectsAsync(string accountId, ProjectQuery query, CancellationToken ct = default);

    Task<Client> CreateClientAsync(string accountId, string name, string? email, string? phone, string? address, string createdBy, CancellationToken ct = default);
    Task<Client?> GetClientAsync(string accountId, string clientId, CancellationToken ct = default);
    Task<Client> UpdateClientAsync(string accountId, string clientId, string name, string? email, string? phone, string? address, string updatedBy, CancellationToken ct = default);
    Task<IReadOnlyList<Client>> ListClientsAsync(string accountId, string? query, int? skip, int? take, CancellationToken ct = default);
    Task<ClientNote> AddClientNoteAsync(string accountId, string clientId, string content, string createdBy, CancellationToken ct = default);
    Task<IReadOnlyList<ClientNote>> ListClientNotesAsync(string accountId, string clientId, CancellationToken ct = default);
    Task<IReadOnlyList<Project>> ListProjectsForClientAsync(string accountId, string clientId, CancellationToken ct = default);

    Task<SearchResult> SearchAsync(string accountId, string query, CancellationToken ct = default);
}

public sealed class ProjectQuery
{
    public int? Skip { get; init; }
    public int? Take { get; init; }
    public bool? Archived { get; init; }
    public IReadOnlyList<string>? Tags { get; init; }
    public DateTimeOffset? DateFrom { get; init; }
    public DateTimeOffset? DateTo { get; init; }
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
}

public sealed class SearchResult
{
    public IReadOnlyList<Project> Projects { get; init; } = Array.Empty<Project>();
    public IReadOnlyList<Client> Clients { get; init; } = Array.Empty<Client>();
}
