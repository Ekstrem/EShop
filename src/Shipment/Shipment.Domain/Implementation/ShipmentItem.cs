using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Implementation;

/// <summary>
/// Immutable implementation of the shipment item value object.
/// </summary>
internal sealed class ShipmentItem : IShipmentItem
{
    public Guid VariantId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }

    private ShipmentItem() { }

    public static IShipmentItem CreateInstance(
        Guid variantId,
        string productName,
        int quantity)
        => new ShipmentItem
        {
            VariantId = variantId,
            ProductName = productName,
            Quantity = quantity
        };
}
