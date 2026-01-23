using PhotographerPlatform.Workspace.Core.Models;
using PhotographerPlatform.Workspace.Core.Repositories;

namespace PhotographerPlatform.Workspace.Core.Services;

public sealed class WorkspaceService : IWorkspaceService
{
    private readonly IProjectRepository _projects;
    private readonly IClientRepository _clients;
    private readonly IClientNoteRepository _notes;
    private readonly ITagRepository _tags;

    public WorkspaceService(
        IProjectRepository projects,
        IClientRepository clients,
        IClientNoteRepository notes,
        ITagRepository tags)
    {
        _projects = projects;
        _clients = clients;
        _notes = notes;
        _tags = tags;
    }

    public async Task<Project> CreateProjectAsync(string accountId, string name, DateTimeOffset? eventDate, string? location, IReadOnlyList<string>? tags, string createdBy, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Project name is required.", nameof(name));
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var project = new Project
        {
            ProjectId = Guid.NewGuid().ToString("N"),
            AccountId = accountId,
            Name = name,
            EventDate = eventDate,
            Location = location,
            Tags = tags?.Distinct(StringComparer.OrdinalIgnoreCase).ToList() ?? new List<string>(),
            Audit = new AuditFields
            {
                CreatedBy = createdBy,
                CreatedAtUnixMs = now,
                UpdatedBy = createdBy,
                UpdatedAtUnixMs = now
            }
        };

        await _projects.AddAsync(project, ct).ConfigureAwait(false);
        await EnsureTagsAsync(accountId, project.Tags, ct).ConfigureAwait(false);
        return project;
    }

    public Task<Project?> GetProjectAsync(string accountId, string projectId, CancellationToken ct = default)
    {
        return _projects.GetByIdAsync(accountId, projectId, ct);
    }

    public async Task<Project> UpdateProjectAsync(string accountId, string projectId, string name, DateTimeOffset? eventDate, string? location, IReadOnlyList<string>? tags, string updatedBy, CancellationToken ct = default)
    {
        var project = await RequireProjectAsync(accountId, projectId, ct).ConfigureAwait(false);
        project.Name = name;
        project.EventDate = eventDate;
        project.Location = location;
        project.Tags = tags?.Distinct(StringComparer.OrdinalIgnoreCase).ToList() ?? new List<string>();
        project.Audit.UpdatedBy = updatedBy;
        project.Audit.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _projects.UpdateAsync(project, ct).ConfigureAwait(false);
        await EnsureTagsAsync(accountId, project.Tags, ct).ConfigureAwait(false);
        return project;
    }

    public async Task<Project> SetProjectClientsAsync(string accountId, string projectId, IReadOnlyList<ProjectClient> clients, string updatedBy, CancellationToken ct = default)
    {
        var project = await RequireProjectAsync(accountId, projectId, ct).ConfigureAwait(false);
        project.Clients = clients.ToList();
        project.Audit.UpdatedBy = updatedBy;
        project.Audit.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _projects.UpdateAsync(project, ct).ConfigureAwait(false);
        return project;
    }

    public async Task ArchiveProjectAsync(string accountId, string projectId, string updatedBy, CancellationToken ct = default)
    {
        var project = await RequireProjectAsync(accountId, projectId, ct).ConfigureAwait(false);
        project.Archived = true;
        project.Status = ProjectStatus.Archived;
        project.Audit.UpdatedBy = updatedBy;
        project.Audit.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _projects.UpdateAsync(project, ct).ConfigureAwait(false);
    }

    public async Task UnarchiveProjectAsync(string accountId, string projectId, string updatedBy, CancellationToken ct = default)
    {
        var project = await RequireProjectAsync(accountId, projectId, ct).ConfigureAwait(false);
        project.Archived = false;
        project.Status = ProjectStatus.Active;
        project.Audit.UpdatedBy = updatedBy;
        project.Audit.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _projects.UpdateAsync(project, ct).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Project>> ListProjectsAsync(string accountId, ProjectQuery query, CancellationToken ct = default)
    {
        var projects = await _projects.ListAsync(accountId, ct).ConfigureAwait(false);
        var filtered = projects.AsQueryable();

        if (query.Archived.HasValue)
        {
            filtered = filtered.Where(project => project.Archived == query.Archived.Value);
        }

        if (query.Tags is { Count: > 0 })
        {
            filtered = filtered.Where(project => query.Tags.All(tag => project.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase)));
        }

        if (query.DateFrom.HasValue)
        {
            filtered = filtered.Where(project => project.EventDate.HasValue && project.EventDate.Value >= query.DateFrom.Value);
        }

        if (query.DateTo.HasValue)
        {
            filtered = filtered.Where(project => project.EventDate.HasValue && project.EventDate.Value <= query.DateTo.Value);
        }

        filtered = ApplyProjectSort(filtered, query);

        if (query.Skip.HasValue)
        {
            filtered = filtered.Skip(query.Skip.Value);
        }

        if (query.Take.HasValue)
        {
            filtered = filtered.Take(query.Take.Value);
        }

        return filtered.ToList();
    }

    public async Task<Client> CreateClientAsync(string accountId, string name, string? email, string? phone, string? address, string createdBy, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Client name is required.", nameof(name));
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var client = new Client
        {
            ClientId = Guid.NewGuid().ToString("N"),
            AccountId = accountId,
            Name = name,
            Email = email,
            Phone = phone,
            Address = address,
            Audit = new AuditFields
            {
                CreatedBy = createdBy,
                CreatedAtUnixMs = now,
                UpdatedBy = createdBy,
                UpdatedAtUnixMs = now
            }
        };

        await _clients.AddAsync(client, ct).ConfigureAwait(false);
        return client;
    }

    public Task<Client?> GetClientAsync(string accountId, string clientId, CancellationToken ct = default)
    {
        return _clients.GetByIdAsync(accountId, clientId, ct);
    }

    public async Task<Client> UpdateClientAsync(string accountId, string clientId, string name, string? email, string? phone, string? address, string updatedBy, CancellationToken ct = default)
    {
        var client = await RequireClientAsync(accountId, clientId, ct).ConfigureAwait(false);
        client.Name = name;
        client.Email = email;
        client.Phone = phone;
        client.Address = address;
        client.Audit.UpdatedBy = updatedBy;
        client.Audit.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _clients.UpdateAsync(client, ct).ConfigureAwait(false);
        return client;
    }

    public async Task<IReadOnlyList<Client>> ListClientsAsync(string accountId, string? query, int? skip, int? take, CancellationToken ct = default)
    {
        var clients = await _clients.ListAsync(accountId, ct).ConfigureAwait(false);
        var filtered = clients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            filtered = filtered.Where(client => client.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                || (client.Email?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
                || (client.Phone?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        if (skip.HasValue)
        {
            filtered = filtered.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            filtered = filtered.Take(take.Value);
        }

        return filtered.ToList();
    }

    public async Task<ClientNote> AddClientNoteAsync(string accountId, string clientId, string content, string createdBy, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Note content is required.", nameof(content));
        }

        _ = await RequireClientAsync(accountId, clientId, ct).ConfigureAwait(false);
        var note = new ClientNote
        {
            NoteId = Guid.NewGuid().ToString("N"),
            ClientId = clientId,
            AccountId = accountId,
            Content = content,
            CreatedBy = createdBy,
            CreatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        await _notes.AddAsync(note, ct).ConfigureAwait(false);
        return note;
    }

    public Task<IReadOnlyList<ClientNote>> ListClientNotesAsync(string accountId, string clientId, CancellationToken ct = default)
    {
        return _notes.ListByClientAsync(accountId, clientId, ct);
    }

    public async Task<IReadOnlyList<Project>> ListProjectsForClientAsync(string accountId, string clientId, CancellationToken ct = default)
    {
        var projects = await _projects.ListAsync(accountId, ct).ConfigureAwait(false);
        return projects.Where(project => project.Clients.Any(client => client.ClientId == clientId)).ToList();
    }

    public async Task<SearchResult> SearchAsync(string accountId, string query, CancellationToken ct = default)
    {
        var projects = await _projects.ListAsync(accountId, ct).ConfigureAwait(false);
        var clients = await _clients.ListAsync(accountId, ct).ConfigureAwait(false);

        var matchingProjects = projects.Where(project => project.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
            || (project.Location?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
            || project.Tags.Any(tag => tag.Contains(query, StringComparison.OrdinalIgnoreCase))).ToList();

        var matchingClients = clients.Where(client => client.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
            || (client.Email?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
            || (client.Phone?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();

        return new SearchResult
        {
            Projects = matchingProjects,
            Clients = matchingClients
        };
    }

    private static IQueryable<Project> ApplyProjectSort(IQueryable<Project> query, ProjectQuery options)
    {
        return options.SortBy?.ToLowerInvariant() switch
        {
            "date" => options.SortDescending
                ? query.OrderByDescending(project => project.EventDate)
                : query.OrderBy(project => project.EventDate),
            "name" => options.SortDescending
                ? query.OrderByDescending(project => project.Name)
                : query.OrderBy(project => project.Name),
            _ => options.SortDescending
                ? query.OrderByDescending(project => project.Audit.CreatedAtUnixMs)
                : query.OrderBy(project => project.Audit.CreatedAtUnixMs)
        };
    }

    private async Task<Project> RequireProjectAsync(string accountId, string projectId, CancellationToken ct)
    {
        var project = await _projects.GetByIdAsync(accountId, projectId, ct).ConfigureAwait(false);
        if (project is null)
        {
            throw new InvalidOperationException("Project not found.");
        }

        return project;
    }

    private async Task<Client> RequireClientAsync(string accountId, string clientId, CancellationToken ct)
    {
        var client = await _clients.GetByIdAsync(accountId, clientId, ct).ConfigureAwait(false);
        if (client is null)
        {
            throw new InvalidOperationException("Client not found.");
        }

        return client;
    }

    private async Task EnsureTagsAsync(string accountId, IReadOnlyList<string> tags, CancellationToken ct)
    {
        if (tags.Count == 0)
        {
            return;
        }

        var existing = await _tags.ListAsync(accountId, ct).ConfigureAwait(false);
        var existingNames = new HashSet<string>(existing.Select(tag => tag.Name), StringComparer.OrdinalIgnoreCase);
        foreach (var tagName in tags)
        {
            if (existingNames.Contains(tagName))
            {
                continue;
            }

            var tag = new Tag
            {
                TagId = Guid.NewGuid().ToString("N"),
                AccountId = accountId,
                Name = tagName
            };
            await _tags.AddAsync(tag, ct).ConfigureAwait(false);
        }
    }
}
