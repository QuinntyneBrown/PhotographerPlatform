using PhotographerPlatform.Communication.Core.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PhotographerPlatform.Communication.Infrastructure.Email;

public sealed class SendGridEmailSender : IEmailSender
{
    private readonly SendGridClient _client;
    private readonly SendGridOptions _options;

    public SendGridEmailSender(SendGridClient client, SendGridOptions options)
    {
        _client = client;
        _options = options;
    }

    public async Task<EmailSendResult> SendAsync(EmailSendRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_options.FromEmail))
        {
            throw new InvalidOperationException("SendGrid FromEmail is not configured.");
        }

        var from = new EmailAddress(_options.FromEmail, _options.FromName);
        var to = new EmailAddress(request.Recipient);
        var message = MailHelper.CreateSingleEmail(from, to, request.Subject, plainTextContent: request.Body, htmlContent: request.Body);

        if (!string.IsNullOrWhiteSpace(_options.ReplyToEmail))
        {
            message.ReplyTo = new EmailAddress(_options.ReplyToEmail, _options.ReplyToName);
        }

        if (_options.EnableSandbox)
        {
            message.MailSettings = new MailSettings
            {
                SandboxMode = new SandboxMode { Enable = true }
            };
        }

        if (request.Metadata.Count > 0)
        {
            message.CustomArgs = request.Metadata;
        }

        var response = await _client.SendEmailAsync(message, ct).ConfigureAwait(false);
        if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
        {
            return new EmailSendResult { Success = true };
        }

        var body = await response.Body.ReadAsStringAsync(ct).ConfigureAwait(false);
        return new EmailSendResult
        {
            Success = false,
            Error = string.IsNullOrWhiteSpace(body) ? response.StatusCode.ToString() : body
        };
    }
}
