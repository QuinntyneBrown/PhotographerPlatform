namespace PhotographerPlatform.Workspace.Core.Models;

public sealed class ClientNote
{
    public required string NoteId { get; init; }
    public required string ClientId { get; init; }
    public required string AccountId { get; init; }
    public required string Content { get; init; }
    public required string CreatedBy { get; init; }
    public required long CreatedAtUnixMs { get; init; }
}
