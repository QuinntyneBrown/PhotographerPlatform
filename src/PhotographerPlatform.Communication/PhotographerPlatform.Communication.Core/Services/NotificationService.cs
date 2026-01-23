using System.Collections.Concurrent;
using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Repositories;

namespace PhotographerPlatform.Communication.Core.Services;

public sealed class NotificationService : INotificationService
{
    private readonly IEmailTemplateRepository _templates;
    private readonly IEmailLogRepository _logs;
    private readonly IEmailSender _sender;
    private readonly ITemplateRenderer _renderer;
    private readonly ConcurrentDictionary<string, List<long>> _recipientLog = new();

    public NotificationService(
        IEmailTemplateRepository templates,
        IEmailLogRepository logs,
        IEmailSender sender,
        ITemplateRenderer renderer)
    {
        _templates = templates;
        _logs = logs;
        _sender = sender;
        _renderer = renderer;
    }

    public async Task<EmailLog> SendAsync(ManualNotificationRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Recipient))
        {
            throw new ArgumentException("Recipient is required.", nameof(request.Recipient));
        }

        var template = await ResolveTemplateAsync(request.TemplateId, request.TemplateType, ct).ConfigureAwait(false);
        var rendered = _renderer.Render(template, request.Variables);
        EnforceThrottle(request.Recipient);

        var log = new EmailLog
        {
            EmailLogId = Guid.NewGuid().ToString("N"),
            Recipient = request.Recipient,
            TemplateType = template.Type,
            TemplateId = template.TemplateId,
            Status = EmailStatus.Pending,
            CreatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Metadata = new Dictionary<string, string>(request.Metadata)
        };

        await _logs.AddAsync(log, ct).ConfigureAwait(false);

        var result = await _sender.SendAsync(new EmailSendRequest
        {
            Recipient = request.Recipient,
            Subject = rendered.Subject,
            Body = rendered.Body,
            TemplateType = template.Type,
            TemplateId = template.TemplateId,
            Metadata = request.Metadata
        }, ct).ConfigureAwait(false);

        log.Status = result.Success ? EmailStatus.Sent : EmailStatus.Failed;
        log.SentAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        log.ErrorMessage = result.Error;
        await _logs.AddAsync(log, ct).ConfigureAwait(false);
        return log;
    }

    public async Task<RenderedTemplate> PreviewAsync(PreviewRequest request, CancellationToken ct = default)
    {
        var template = await ResolveTemplateAsync(request.TemplateId, request.TemplateType, ct).ConfigureAwait(false);
        return _renderer.Render(template, request.Variables);
    }

    public Task<IReadOnlyList<EmailTemplate>> ListTemplatesAsync(CancellationToken ct = default)
    {
        return _templates.ListAsync(ct);
    }

    public async Task<EmailTemplate> UpdateTemplateAsync(string templateId, string subject, string body, List<string> variables, CancellationToken ct = default)
    {
        var template = await _templates.GetByIdAsync(templateId, ct).ConfigureAwait(false);
        if (template is null)
        {
            throw new InvalidOperationException("Template not found.");
        }

        template.Subject = subject;
        template.Body = body;
        template.Variables = variables;
        template.Version += 1;
        template.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _templates.AddOrUpdateAsync(template, ct).ConfigureAwait(false);
        return template;
    }

    public Task<IReadOnlyList<EmailLog>> ListLogsAsync(int? take, CancellationToken ct = default)
    {
        return _logs.ListAsync(take, ct);
    }

    private async Task<EmailTemplate> ResolveTemplateAsync(string? templateId, EmailTemplateType? type, CancellationToken ct)
    {
        EmailTemplate? template = null;
        if (!string.IsNullOrWhiteSpace(templateId))
        {
            template = await _templates.GetByIdAsync(templateId, ct).ConfigureAwait(false);
        }
        else if (type.HasValue)
        {
            template = await _templates.GetByTypeAsync(type.Value, ct).ConfigureAwait(false);
        }

        if (template is null)
        {
            throw new InvalidOperationException("Template not found.");
        }

        return template;
    }

    private void EnforceThrottle(string recipient)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var windowStart = now - (60 * 1000);
        var list = _recipientLog.GetOrAdd(recipient, _ => new List<long>());
        lock (list)
        {
            list.RemoveAll(timestamp => timestamp < windowStart);
            if (list.Count >= 5)
            {
                throw new InvalidOperationException("Recipient has exceeded notification throttle.");
            }

            list.Add(now);
        }
    }
}
