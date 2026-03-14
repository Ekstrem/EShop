using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Implementation;

/// <summary>
/// Immutable implementation of the Shipment aggregate root.
/// </summary>
internal sealed class ShipmentRoot : IShipmentRoot
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string TrackingNumber { get; private set; } = string.Empty;
    public string Carrier { get; private set; } = string.Empty;
    public string ShippingAddress { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Pending";
    public DateTime CreatedAt { get; private set; }

    private ShipmentRoot() { }

    public static IShipmentRoot CreateInstance(
        Guid id,
        Guid orderId,
        string trackingNumber,
        string carrier,
        string shippingAddress,
        string status,
        DateTime createdAt)
        => new ShipmentRoot
        {
            Id = id,
            OrderId = orderId,
            TrackingNumber = trackingNumber,
            Carrier = carrier,
            ShippingAddress = shippingAddress,
            Status = status,
            CreatedAt = createdAt
        };
}
