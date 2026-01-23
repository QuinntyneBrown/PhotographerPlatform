namespace PhotographerPlatform.Workspace.Core.Models;

public sealed class Project
{
    public required string ProjectId { get; init; }
    public required string AccountId { get; init; }
    public required string Name { get; set; }
    public DateTimeOffset? EventDate { get; set; }
    public string? Location { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;
    public bool Archived { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<ProjectClient> Clients { get; set; } = new();
    public AuditFields Audit { get; init; } = default!;
}
