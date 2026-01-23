namespace PhotographerPlatform.Communication.Core.Models;

public sealed class Note
{
    public required string NoteId { get; init; }
    public required string ProjectId { get; init; }
    public required string AuthorId { get; init; }
    public required string Content { get; set; }
    public NoteCategory Category { get; set; } = NoteCategory.General;
    public bool InternalOnly { get; set; } = true;
    public long CreatedAtUnixMs { get; init; }
    public long? UpdatedAtUnixMs { get; set; }
}
