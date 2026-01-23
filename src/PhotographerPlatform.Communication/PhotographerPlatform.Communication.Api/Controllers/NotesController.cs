using Microsoft.AspNetCore.Mvc;
using PhotographerPlatform.Communication.Api.Models;
using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Services;

namespace PhotographerPlatform.Communication.Api.Controllers;

[ApiController]
public sealed class NotesController : ControllerBase
{
    private readonly IMessagingService _messaging;

    public NotesController(IMessagingService messaging)
    {
        _messaging = messaging;
    }

    [HttpGet("/api/projects/{id}/notes")]
    public async Task<ActionResult<IReadOnlyList<Note>>> ListByProject(string id, CancellationToken ct)
    {
        var notes = await _messaging.ListNotesAsync(id, ct).ConfigureAwait(false);
        return Ok(notes);
    }

    [HttpPost("/api/projects/{id}/notes")]
    public async Task<ActionResult<Note>> Add(string id, [FromBody] NoteCreateRequest request, CancellationToken ct)
    {
        var note = await _messaging.AddNoteAsync(id, request.AuthorId, request.Content, request.Category, request.InternalOnly, ct)
            .ConfigureAwait(false);
        return Ok(note);
    }

    [HttpPut("/api/notes/{id}")]
    public async Task<ActionResult<Note>> Update(string id, [FromBody] NoteUpdateRequest request, CancellationToken ct)
    {
        var note = await _messaging.UpdateNoteAsync(id, request.Content, request.Category, request.InternalOnly, ct)
            .ConfigureAwait(false);
        return Ok(note);
    }

    [HttpDelete("/api/notes/{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _messaging.DeleteNoteAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }
}
