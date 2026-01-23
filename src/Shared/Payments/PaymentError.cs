namespace Shared.Payments;

public sealed class PaymentError
{
    public required string Code { get; init; }
    public required string Message { get; init; }
    public bool IsRetryable { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = new();
}
