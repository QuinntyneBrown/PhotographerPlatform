using MessagePack;

namespace Shared.LabFulfillment;

[MessagePackObject]
public sealed class PrintJobRequest
{
    [Key(0)]
    public required string PrintJobRequestId { get; init; }

    [Key(1)]
    public required string OrderId { get; init; }

    [Key(2)]
    public required string CustomerId { get; init; }

    [Key(3)]
    public required List<PrintProduct> Products { get; init; }

    [Key(4)]
    public required ShippingAddress ShippingAddress { get; init; }

    [Key(5)]
    public required string ShippingMethod { get; init; }

    [Key(6)]
    public string? SpecialInstructions { get; init; }

    [Key(7)]
    public Dictionary<string, string> Metadata { get; init; } = new();
}
