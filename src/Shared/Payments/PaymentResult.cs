using MessagePack;

namespace Shared.Payments;

[MessagePackObject]
public sealed class PaymentResult
{
    [Key(0)]
    public required string PaymentId { get; init; }

    [Key(1)]
    public required string PaymentRequestId { get; init; }

    [Key(2)]
    public required PaymentStatus Status { get; init; }

    [Key(3)]
    public required decimal Amount { get; init; }

    [Key(4)]
    public required string Currency { get; init; }

    [Key(5)]
    public string? TransactionId { get; init; }

    [Key(6)]
    public string? ErrorCode { get; init; }

    [Key(7)]
    public string? ErrorMessage { get; init; }

    [Key(8)]
    public long ProcessedAtUnixMs { get; init; }

    [Key(9)]
    public Dictionary<string, string> ProviderMetadata { get; init; } = new();
}
