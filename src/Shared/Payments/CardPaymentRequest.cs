using MessagePack;

namespace Shared.Payments;

[MessagePackObject]
public sealed class CardPaymentRequest
{
    [Key(0)]
    public required string PaymentRequestId { get; init; }

    [Key(1)]
    public required decimal Amount { get; init; }

    [Key(2)]
    public required string Currency { get; init; }

    [Key(3)]
    public required string CardToken { get; init; }

    [Key(4)]
    public required string CustomerId { get; init; }

    [Key(5)]
    public string? Description { get; init; }

    [Key(6)]
    public Dictionary<string, string> Metadata { get; init; } = new();
}
