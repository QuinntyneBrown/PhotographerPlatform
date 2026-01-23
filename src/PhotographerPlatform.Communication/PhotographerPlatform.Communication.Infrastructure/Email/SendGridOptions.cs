namespace PhotographerPlatform.Communication.Infrastructure.Email;

public sealed class SendGridOptions
{
    public string? ApiKey { get; init; }
    public string? FromEmail { get; init; }
    public string? FromName { get; init; }
    public string? ReplyToEmail { get; init; }
    public string? ReplyToName { get; init; }
    public bool EnableSandbox { get; init; }
}
