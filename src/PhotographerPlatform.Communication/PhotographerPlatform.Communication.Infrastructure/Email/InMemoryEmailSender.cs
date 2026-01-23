using PhotographerPlatform.Communication.Core.Services;

namespace PhotographerPlatform.Communication.Infrastructure.Email;

public sealed class InMemoryEmailSender : IEmailSender
{
    public Task<EmailSendResult> SendAsync(EmailSendRequest request, CancellationToken ct = default)
    {
        if (request.Recipient.Contains("fail", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(new EmailSendResult
            {
                Success = false,
                Error = "Simulated delivery failure."
            });
        }

        return Task.FromResult(new EmailSendResult { Success = true });
    }
}
