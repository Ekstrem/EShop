namespace Shipment.Storage.Entities;

/// <summary>
/// Persistence entity for the Shipment aggregate.
/// </summary>
public sealed class ShipmentEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
    public string Carrier { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
    public string? LabelUrl { get; set; }
    public DateTime? LabelGeneratedAt { get; set; }
    public List<ShipmentItemEntity> Items { get; set; } = new();
}
