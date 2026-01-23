namespace PhotographerPlatform.Workspace.Core.Models;

public sealed class Client
{
    public required string ClientId { get; init; }
    public required string AccountId { get; init; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool Archived { get; set; }
    public AuditFields Audit { get; init; } = default!;
}
