namespace Shared.Reliability;

public sealed class StartupValidator
{
    private readonly List<string> _missing = new();

    public StartupValidator Require(string name, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _missing.Add(name);
        }

        return this;
    }

    public StartupValidator Require(string name, int? value)
    {
        if (!value.HasValue)
        {
            _missing.Add(name);
        }

        return this;
    }

    public void ThrowIfMissing()
    {
        if (_missing.Count == 0)
        {
            return;
        }

        throw new InvalidOperationException($"Missing required configuration values: {string.Join(", ", _missing)}");
    }
}
