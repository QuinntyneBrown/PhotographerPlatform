namespace Shared.Cdn;

public sealed class CacheControlPolicy
{
    public int MaxAgeSeconds { get; init; } = 3600;
    public int? SharedMaxAgeSeconds { get; init; }
    public bool Public { get; init; } = true;
    public bool Immutable { get; init; }
    public bool NoTransform { get; init; } = true;

    public string ToHeaderValue()
    {
        var directives = new List<string>();
        directives.Add(Public ? "public" : "private");
        directives.Add($"max-age={MaxAgeSeconds}");
        if (SharedMaxAgeSeconds.HasValue)
        {
            directives.Add($"s-maxage={SharedMaxAgeSeconds.Value}");
        }

        if (Immutable)
        {
            directives.Add("immutable");
        }

        if (NoTransform)
        {
            directives.Add("no-transform");
        }

        return string.Join(", ", directives);
    }
}
