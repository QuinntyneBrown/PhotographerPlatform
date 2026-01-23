using MessagePack;

namespace Shared.LabFulfillment;

[MessagePackObject]
public sealed class LabQuoteResult
{
    [Key(0)]
    public required string QuoteRequestId { get; init; }

    [Key(1)]
    public required decimal TotalAmount { get; init; }

    [Key(2)]
    public required string Currency { get; init; }

    [Key(3)]
    public required IReadOnlyList<LabQuoteLineItem> LineItems { get; init; }

    [Key(4)]
    public string? ErrorCode { get; init; }

    [Key(5)]
    public string? ErrorMessage { get; init; }

    [Key(6)]
    public Dictionary<string, string> LabMetadata { get; init; } = new();
}

[MessagePackObject]
public sealed class LabQuoteLineItem
{
    [Key(0)]
    public required string ProductId { get; init; }

    [Key(1)]
    public required int Quantity { get; init; }

    [Key(2)]
    public required decimal UnitPrice { get; init; }

    [Key(3)]
    public decimal? ShippingAmount { get; init; }

    [Key(4)]
    public decimal? TaxAmount { get; init; }
}
