using Microsoft.AspNetCore.Mvc;
using PhotographerPlatform.Workspace.Api.Models;
using PhotographerPlatform.Workspace.Api.Services;
using PhotographerPlatform.Workspace.Core.Models;
using PhotographerPlatform.Workspace.Core.Services;

namespace PhotographerPlatform.Workspace.Api.Controllers;

[ApiController]
[Route("api/clients")]
public sealed class ClientsController : ControllerBase
{
    private readonly IWorkspaceService _workspace;

    public ClientsController(IWorkspaceService workspace)
    {
        _workspace = workspace;
    }

    [HttpPost]
    public async Task<ActionResult<Client>> Create([FromBody] ClientCreateRequest request, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var userId = RequestContext.GetUserId(Request);
        var client = await _workspace.CreateClientAsync(accountId, request.Name, request.Email, request.Phone, request.Address, userId, ct)
            .ConfigureAwait(false);
        return Ok(client);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Client>>> List([FromQuery] string? q, [FromQuery] int? skip, [FromQuery] int? take, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var clients = await _workspace.ListClientsAsync(accountId, q, skip, take, ct).ConfigureAwait(false);
        return Ok(clients);
    }

    [HttpGet("{clientId}")]
    public async Task<ActionResult<Client>> GetById(string clientId, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var client = await _workspace.GetClientAsync(accountId, clientId, ct).ConfigureAwait(false);
        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpPut("{clientId}")]
    public async Task<ActionResult<Client>> Update(string clientId, [FromBody] ClientUpdateRequest request, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var userId = RequestContext.GetUserId(Request);
        var client = await _workspace.UpdateClientAsync(accountId, clientId, request.Name, request.Email, request.Phone, request.Address, userId, ct)
            .ConfigureAwait(false);
        return Ok(client);
    }

    [HttpPost("{clientId}/notes")]
    public async Task<ActionResult<ClientNote>> AddNote(string clientId, [FromBody] ClientNoteRequest request, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var userId = RequestContext.GetUserId(Request);
        var note = await _workspace.AddClientNoteAsync(accountId, clientId, request.Content, userId, ct).ConfigureAwait(false);
        return Ok(note);
    }

    [HttpGet("{clientId}/notes")]
    public async Task<ActionResult<IReadOnlyList<ClientNote>>> ListNotes(string clientId, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var notes = await _workspace.ListClientNotesAsync(accountId, clientId, ct).ConfigureAwait(false);
        return Ok(notes);
    }

    [HttpGet("{clientId}/projects")]
    public async Task<ActionResult<IReadOnlyList<Project>>> ListProjects(string clientId, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        var projects = await _workspace.ListProjectsForClientAsync(accountId, clientId, ct).ConfigureAwait(false);
        return Ok(projects);
    }
}
