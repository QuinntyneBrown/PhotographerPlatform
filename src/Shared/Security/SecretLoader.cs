namespace Shared.Security;

public sealed class SecretReference
{
    public string? Value { get; init; }
    public string? EnvironmentVariable { get; init; }
    public string? FilePath { get; init; }

    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Value)
            && string.IsNullOrWhiteSpace(EnvironmentVariable)
            && string.IsNullOrWhiteSpace(FilePath);
    }
}

public static class SecretLoader
{
    public static string Resolve(SecretReference reference)
    {
        var secret = TryResolve(reference);
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException("Secret value could not be resolved.");
        }

        return secret;
    }

    public static string? TryResolve(SecretReference reference)
    {
        if (!string.IsNullOrWhiteSpace(reference.Value))
        {
            return reference.Value;
        }

        if (!string.IsNullOrWhiteSpace(reference.EnvironmentVariable))
        {
            var envValue = Environment.GetEnvironmentVariable(reference.EnvironmentVariable);
            if (!string.IsNullOrWhiteSpace(envValue))
            {
                return envValue;
            }
        }

        if (!string.IsNullOrWhiteSpace(reference.FilePath) && File.Exists(reference.FilePath))
        {
            var fileValue = File.ReadAllText(reference.FilePath).Trim();
            if (!string.IsNullOrWhiteSpace(fileValue))
            {
                return fileValue;
            }
        }

        return null;
    }
}
