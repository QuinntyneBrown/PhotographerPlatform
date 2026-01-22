using MessagePack;

namespace Shared.LabFulfillment;

[MessagePackObject]
public sealed class ShippingAddress
{
    [Key(0)]
    public required string RecipientName { get; init; }

    [Key(1)]
    public required string AddressLine1 { get; init; }

    [Key(2)]
    public string? AddressLine2 { get; init; }

    [Key(3)]
    public required string City { get; init; }

    [Key(4)]
    public required string StateOrProvince { get; init; }

    [Key(5)]
    public required string PostalCode { get; init; }

    [Key(6)]
    public required string CountryCode { get; init; }

    [Key(7)]
    public string? PhoneNumber { get; init; }
}
