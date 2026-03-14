namespace Shipment.InternalContracts;

/// <summary>
/// Read model for shipment queries.
/// </summary>
public sealed class ShipmentReadModel
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public string TrackingNumber { get; init; } = string.Empty;
    public string Carrier { get; init; } = string.Empty;
    public string ShippingAddress { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public IReadOnlyList<ShipmentItemReadModel> Items { get; init; } = new List<ShipmentItemReadModel>();
    public ShippingLabelReadModel? Label { get; init; }
}

/// <summary>
/// Read model for a shipment item.
/// </summary>
public sealed class ShipmentItemReadModel
{
    public Guid VariantId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
}

/// <summary>
/// Read model for a shipping label.
/// </summary>
public sealed class ShippingLabelReadModel
{
    public string LabelUrl { get; init; } = string.Empty;
    public DateTime GeneratedAt { get; init; }
}
