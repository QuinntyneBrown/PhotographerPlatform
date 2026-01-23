using Microsoft.AspNetCore.Mvc;
using PhotographerPlatform.Workspace.Api.Models;
using PhotographerPlatform.Workspace.Api.Services;
using PhotographerPlatform.Workspace.Core.Models;
using PhotographerPlatform.Workspace.Core.Services;

namespace PhotographerPlatform.Workspace.Api.Controllers;

[ApiController]
[Route("api/projects")]
public sealed class ProjectsController : ControllerBase
{
    private readonly IWorkspaceService _workspace;

    public ProjectsController(IWorkspaceService workspace)
    {
        _workspace = workspace;
    }

    [HttpPost]
    public async Task<ActionResult<Project>> Create([FromBody] ProjectCreateRequest request, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var userId = RequestContext.GetUserId(Request);
        var project = await _workspace.CreateProjectAsync(accountId, request.Name, request.EventDate, request.Location, request.Tags, userId, ct)
            .ConfigureAwait(false);

        if (request.Clients.Count > 0)
        {
            var clients = request.Clients.Select(client => new ProjectClient { ClientId = client.ClientId, IsPrimary = client.IsPrimary }).ToList();
            project = await _workspace.SetProjectClientsAsync(accountId, project.ProjectId, clients, userId, ct).ConfigureAwait(false);
        }

        return Ok(project);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Project>>> List(
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromQuery] bool? archived,
        [FromQuery] string? tags,
        [FromQuery] DateTimeOffset? dateFrom,
        [FromQuery] DateTimeOffset? dateTo,
        [FromQuery] string? sortBy,
        [FromQuery] bool sortDesc,
        CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var query = new ProjectQuery
        {
            Skip = skip,
            Take = take,
            Archived = archived,
            Tags = string.IsNullOrWhiteSpace(tags)
                ? null
                : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            DateFrom = dateFrom,
            DateTo = dateTo,
            SortBy = sortBy,
            SortDescending = sortDesc
        };

        var projects = await _workspace.ListProjectsAsync(accountId, query, ct).ConfigureAwait(false);
        return Ok(projects);
    }

    [HttpGet("{projectId}")]
    public async Task<ActionResult<Project>> GetById(string projectId, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var project = await _workspace.GetProjectAsync(accountId, projectId, ct).ConfigureAwait(false);
        if (project is null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    [HttpPut("{projectId}")]
    public async Task<ActionResult<Project>> Update(string projectId, [FromBody] ProjectUpdateRequest request, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var userId = RequestContext.GetUserId(Request);
        var project = await _workspace.UpdateProjectAsync(accountId, projectId, request.Name, request.EventDate, request.Location, request.Tags, userId, ct)
            .ConfigureAwait(false);

        var clients = request.Clients.Select(client => new ProjectClient { ClientId = client.ClientId, IsPrimary = client.IsPrimary }).ToList();
        if (clients.Count > 0)
        {
            project = await _workspace.SetProjectClientsAsync(accountId, projectId, clients, userId, ct).ConfigureAwait(false);
        }

        return Ok(project);
    }

    [HttpPost("{projectId}/archive")]
    public async Task<IActionResult> Archive(string projectId, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var userId = RequestContext.GetUserId(Request);
        await _workspace.ArchiveProjectAsync(accountId, projectId, userId, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPost("{projectId}/unarchive")]
    public async Task<IActionResult> Unarchive(string projectId, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var userId = RequestContext.GetUserId(Request);
        await _workspace.UnarchiveProjectAsync(accountId, projectId, userId, ct).ConfigureAwait(false);
        return NoContent();
    }
}
