using Microsoft.AspNetCore.Mvc;
using PhotographerPlatform.Communication.Api.Models;
using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Services;

namespace PhotographerPlatform.Communication.Api.Controllers;

[ApiController]
public sealed class MessagesController : ControllerBase
{
    private readonly IMessagingService _messaging;

    public MessagesController(IMessagingService messaging)
    {
        _messaging = messaging;
    }

    [HttpGet("/api/projects/{id}/messages")]
    public async Task<ActionResult<IReadOnlyList<Message>>> ListByProject(string id, CancellationToken ct)
    {
        var messages = await _messaging.ListMessagesAsync(id, ct).ConfigureAwait(false);
        return Ok(messages);
    }

    [HttpPost("/api/projects/{id}/messages")]
    public async Task<ActionResult<Message>> Add(string id, [FromBody] MessageCreateRequest request, CancellationToken ct)
    {
        var message = await _messaging.AddMessageAsync(id, request.SenderId, request.Content, request.Visibility, request.ThreadId, ct)
            .ConfigureAwait(false);
        return Ok(message);
    }

    [HttpPut("/api/messages/{id}")]
    public async Task<ActionResult<Message>> Update(string id, [FromBody] MessageUpdateRequest request, CancellationToken ct)
    {
        var message = await _messaging.UpdateMessageAsync(id, request.Content, ct).ConfigureAwait(false);
        return Ok(message);
    }

    [HttpDelete("/api/messages/{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _messaging.DeleteMessageAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPut("/api/messages/{id}/visibility")]
    public async Task<ActionResult<Message>> SetVisibility(string id, [FromBody] MessageVisibilityRequest request, CancellationToken ct)
    {
        var message = await _messaging.SetVisibilityAsync(id, request.Visibility, ct).ConfigureAwait(false);
        return Ok(message);
    }
}
