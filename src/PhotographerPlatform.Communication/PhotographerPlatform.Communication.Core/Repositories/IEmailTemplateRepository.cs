using PhotographerPlatform.Communication.Core.Models;

namespace PhotographerPlatform.Communication.Core.Repositories;

public interface IEmailTemplateRepository
{
    Task<EmailTemplate?> GetByIdAsync(string templateId, CancellationToken ct = default);
    Task<EmailTemplate?> GetByTypeAsync(EmailTemplateType type, CancellationToken ct = default);
    Task<IReadOnlyList<EmailTemplate>> ListAsync(CancellationToken ct = default);
    Task AddOrUpdateAsync(EmailTemplate template, CancellationToken ct = default);
}
