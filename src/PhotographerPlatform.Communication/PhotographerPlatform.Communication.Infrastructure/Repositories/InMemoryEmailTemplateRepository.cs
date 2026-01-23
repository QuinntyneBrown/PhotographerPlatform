using System.Collections.Concurrent;
using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Repositories;

namespace PhotographerPlatform.Communication.Infrastructure.Repositories;

public sealed class InMemoryEmailTemplateRepository : IEmailTemplateRepository
{
    private readonly ConcurrentDictionary<string, EmailTemplate> _templates = new();

    public InMemoryEmailTemplateRepository()
    {
        SeedDefaults();
    }

    public Task<EmailTemplate?> GetByIdAsync(string templateId, CancellationToken ct = default)
    {
        _templates.TryGetValue(templateId, out var template);
        return Task.FromResult(template);
    }

    public Task<EmailTemplate?> GetByTypeAsync(EmailTemplateType type, CancellationToken ct = default)
    {
        var template = _templates.Values.FirstOrDefault(item => item.Type == type);
        return Task.FromResult(template);
    }

    public Task<IReadOnlyList<EmailTemplate>> ListAsync(CancellationToken ct = default)
    {
        return Task.FromResult<IReadOnlyList<EmailTemplate>>(_templates.Values.ToList());
    }

    public Task AddOrUpdateAsync(EmailTemplate template, CancellationToken ct = default)
    {
        _templates[template.TemplateId] = template;
        return Task.CompletedTask;
    }

    private void SeedDefaults()
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        foreach (var template in DefaultTemplates(now))
        {
            _templates[template.TemplateId] = template;
        }
    }

    private static IEnumerable<EmailTemplate> DefaultTemplates(long now)
    {
        return new[]
        {
            CreateTemplate(EmailTemplateType.GalleryDelivery, "Your gallery is ready", "Hi {{clientName}}, your gallery {{galleryName}} is ready: {{galleryLink}}", now),
            CreateTemplate(EmailTemplateType.OrderConfirmation, "Order confirmation", "Thanks {{clientName}} for your order {{orderId}}.", now),
            CreateTemplate(EmailTemplateType.OrderShipped, "Your order shipped", "Good news {{clientName}}, order {{orderId}} shipped. Tracking: {{trackingNumber}}", now),
            CreateTemplate(EmailTemplateType.OrderDelivered, "Your order delivered", "Order {{orderId}} was delivered. Enjoy!", now),
            CreateTemplate(EmailTemplateType.GalleryExpiring, "Gallery expiring soon", "Your gallery {{galleryName}} expires on {{expirationDate}}.", now),
            CreateTemplate(EmailTemplateType.InvoiceReminder, "Invoice reminder", "Invoice {{invoiceId}} is due on {{dueDate}}.", now),
            CreateTemplate(EmailTemplateType.NewMessage, "New message", "You have a new message on project {{projectName}}.", now),
            CreateTemplate(EmailTemplateType.Welcome, "Welcome", "Welcome {{clientName}} to PhotographerPlatform!", now)
        };
    }

    private static EmailTemplate CreateTemplate(EmailTemplateType type, string subject, string body, long now)
    {
        return new EmailTemplate
        {
            TemplateId = Guid.NewGuid().ToString("N"),
            Type = type,
            Subject = subject,
            Body = body,
            Variables = ExtractVariables(subject, body),
            CreatedAtUnixMs = now,
            UpdatedAtUnixMs = now
        };
    }

    private static List<string> ExtractVariables(string subject, string body)
    {
        var tokens = new List<string>();
        tokens.AddRange(FindTokens(subject));
        tokens.AddRange(FindTokens(body));
        return tokens.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static IEnumerable<string> FindTokens(string input)
    {
        var matches = System.Text.RegularExpressions.Regex.Matches(input, "{{\\s*(?<key>[^\\s}]+)\\s*}}");
        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            yield return match.Groups["key"].Value;
        }
    }
}
