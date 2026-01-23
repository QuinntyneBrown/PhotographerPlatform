using Microsoft.AspNetCore.Mvc;
using PhotographerPlatform.Communication.Api.Models;
using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Services;

namespace PhotographerPlatform.Communication.Api.Controllers;

[ApiController]
[Route("api/notifications")]
public sealed class NotificationsController : ControllerBase
{
    private readonly INotificationService _notifications;

    public NotificationsController(INotificationService notifications)
    {
        _notifications = notifications;
    }

    [HttpPost("send")]
    public async Task<ActionResult<EmailLog>> Send([FromBody] SendNotificationRequest request, CancellationToken ct)
    {
        var result = await _notifications.SendAsync(new ManualNotificationRequest
        {
            Recipient = request.Recipient,
            TemplateId = request.TemplateId,
            TemplateType = request.TemplateType,
            Variables = request.Variables,
            Metadata = request.Metadata
        }, ct).ConfigureAwait(false);

        return Ok(result);
    }

    [HttpGet("templates")]
    public async Task<ActionResult<IReadOnlyList<EmailTemplate>>> ListTemplates(CancellationToken ct)
    {
        var templates = await _notifications.ListTemplatesAsync(ct).ConfigureAwait(false);
        return Ok(templates);
    }

    [HttpPut("templates/{id}")]
    public async Task<ActionResult<EmailTemplate>> UpdateTemplate(string id, [FromBody] UpdateTemplateRequest request, CancellationToken ct)
    {
        var template = await _notifications.UpdateTemplateAsync(id, request.Subject, request.Body, request.Variables, ct)
            .ConfigureAwait(false);
        return Ok(template);
    }

    [HttpGet("logs")]
    public async Task<ActionResult<IReadOnlyList<EmailLog>>> ListLogs([FromQuery] int? take, CancellationToken ct)
    {
        var logs = await _notifications.ListLogsAsync(take, ct).ConfigureAwait(false);
        return Ok(logs);
    }

    [HttpPost("preview")]
    public async Task<ActionResult<RenderedTemplate>> Preview([FromBody] PreviewTemplateRequest request, CancellationToken ct)
    {
        var preview = await _notifications.PreviewAsync(new PreviewRequest
        {
            TemplateId = request.TemplateId,
            TemplateType = request.TemplateType,
            Variables = request.Variables
        }, ct).ConfigureAwait(false);
        return Ok(preview);
    }
}
