using MessagePack;

namespace Shared.LabFulfillment;

[MessagePackObject]
public sealed class LabOrderResult
{
    [Key(0)]
    public required string LabOrderId { get; init; }

    [Key(1)]
    public required string PrintJobRequestId { get; init; }

    [Key(2)]
    public required LabOrderStatus Status { get; init; }

    [Key(3)]
    public string? LabReferenceNumber { get; init; }

    [Key(4)]
    public string? TrackingNumber { get; init; }

    [Key(5)]
    public string? TrackingUrl { get; init; }

    [Key(6)]
    public string? ErrorCode { get; init; }

    [Key(7)]
    public string? ErrorMessage { get; init; }

    [Key(8)]
    public long? EstimatedDeliveryUnixMs { get; init; }

    [Key(9)]
    public long SubmittedAtUnixMs { get; init; }

    [Key(10)]
    public Dictionary<string, string> LabMetadata { get; init; } = new();
}
