using System.Collections.Concurrent;

namespace Shared.Correlation;

public sealed class CorrelationContext : IDisposable
{
    private static readonly AsyncLocal<string?> CurrentCorrelationId = new();
    private static readonly AsyncLocal<Dictionary<string, string>?> CurrentMetadata = new();

    private readonly string? _previousId;
    private readonly Dictionary<string, string>? _previousMetadata;

    private CorrelationContext(string? previousId, Dictionary<string, string>? previousMetadata)
    {
        _previousId = previousId;
        _previousMetadata = previousMetadata;
    }

    public static string? CorrelationId => CurrentCorrelationId.Value;

    public static IReadOnlyDictionary<string, string> Metadata =>
        CurrentMetadata.Value ?? new Dictionary<string, string>();

    public static CorrelationContext Begin(string correlationId, Dictionary<string, string>? metadata = null)
    {
        var previousId = CurrentCorrelationId.Value;
        var previousMetadata = CurrentMetadata.Value;
        CurrentCorrelationId.Value = correlationId;
        CurrentMetadata.Value = metadata ?? new Dictionary<string, string>();
        return new CorrelationContext(previousId, previousMetadata);
    }

    public void Dispose()
    {
        CurrentCorrelationId.Value = _previousId;
        CurrentMetadata.Value = _previousMetadata;
    }
}
