using PhotographerPlatform.Communication.Core.Models;
using PhotographerPlatform.Communication.Core.Services;
using PhotographerPlatform.Communication.Infrastructure.Email;
using PhotographerPlatform.Communication.Infrastructure.Repositories;
using Xunit;

namespace PhotographerPlatform.Communication.Core.Tests;

public sealed class CommunicationServiceTests
{
    [Fact]
    public void TemplateRenderer_ReplacesVariables()
    {
        var template = new EmailTemplate
        {
            TemplateId = "tmpl",
            Type = EmailTemplateType.Welcome,
            Subject = "Welcome {{name}}",
            Body = "Hello {{name}}",
            Variables = new List<string> { "name" },
            CreatedAtUnixMs = 1,
            UpdatedAtUnixMs = 1
        };

        var renderer = new SimpleTemplateRenderer();
        var rendered = renderer.Render(template, new Dictionary<string, string> { ["name"] = "Alex" });

        Assert.Equal("Welcome Alex", rendered.Subject);
        Assert.Equal("Hello Alex", rendered.Body);
    }

    [Fact]
    public async Task NotificationService_SendsAndLogs()
    {
        var templates = new InMemoryEmailTemplateRepository();
        var logs = new InMemoryEmailLogRepository();
        var sender = new InMemoryEmailSender();
        var renderer = new SimpleTemplateRenderer();
        var service = new NotificationService(templates, logs, sender, renderer);

        var template = await templates.GetByTypeAsync(EmailTemplateType.Welcome) ?? throw new InvalidOperationException();
        var log = await service.SendAsync(new ManualNotificationRequest
        {
            Recipient = "client@example.com",
            TemplateId = template.TemplateId,
            Variables = new Dictionary<string, string> { ["clientName"] = "Alex" }
        });

        Assert.Equal(EmailStatus.Sent, log.Status);
    }

    [Fact]
    public async Task MessagingService_CanUpdateVisibility()
    {
        var messages = new InMemoryMessageRepository();
        var notes = new InMemoryNoteRepository();
        var service = new MessagingService(messages, notes);

        var message = await service.AddMessageAsync("proj", "user", "hello", MessageVisibility.Internal, null);
        var updated = await service.SetVisibilityAsync(message.MessageId, MessageVisibility.Client);

        Assert.Equal(MessageVisibility.Client, updated.Visibility);
    }
}
