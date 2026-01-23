using MessagePack;

namespace Shared.LabFulfillment;

[MessagePackObject]
public sealed class LabQuoteRequest
{
    [Key(0)]
    public required string QuoteRequestId { get; init; }

    [Key(1)]
    public required IReadOnlyList<PrintProduct> Products { get; init; }

    [Key(2)]
    public required string Currency { get; init; }

    [Key(3)]
    public required ShippingAddress ShipTo { get; init; }

    [Key(4)]
    public Dictionary<string, string> Metadata { get; init; } = new();
}
