using MessagePack;

namespace Shared.LabFulfillment;

[MessagePackObject]
public sealed class PrintProduct
{
    [Key(0)]
    public required string ProductId { get; init; }

    [Key(1)]
    public required string ProductSku { get; init; }

    [Key(2)]
    public required string ProductName { get; init; }

    [Key(3)]
    public required int Quantity { get; init; }

    [Key(4)]
    public required string ImageUrl { get; init; }

    [Key(5)]
    public string? PaperType { get; init; }

    [Key(6)]
    public string? Size { get; init; }

    [Key(7)]
    public string? Finish { get; init; }

    [Key(8)]
    public Dictionary<string, string> Options { get; init; } = new();
}
