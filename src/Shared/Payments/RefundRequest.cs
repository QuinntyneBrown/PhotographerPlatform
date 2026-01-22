using MessagePack;

namespace Shared.Payments;

[MessagePackObject]
public sealed class RefundRequest
{
    [Key(0)]
    public required string RefundRequestId { get; init; }

    [Key(1)]
    public required string PaymentId { get; init; }

    [Key(2)]
    public required decimal Amount { get; init; }

    [Key(3)]
    public string? Reason { get; init; }
}
