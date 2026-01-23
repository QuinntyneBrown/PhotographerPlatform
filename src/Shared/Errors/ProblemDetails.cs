namespace Shared.Errors;

public sealed class ProblemDetails
{
    public string Type { get; init; } = "about:blank";
    public string Title { get; init; } = "An error occurred.";
    public int Status { get; init; } = 500;
    public string? Detail { get; init; }
    public string? Instance { get; init; }
    public Dictionary<string, object?> Extensions { get; init; } = new();
}
