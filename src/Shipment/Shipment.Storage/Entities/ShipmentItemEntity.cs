namespace Shipment.Storage.Entities;

/// <summary>
/// Persistence entity for a shipment item.
/// </summary>
public sealed class ShipmentItemEntity
{
    public Guid Id { get; set; }
    public Guid ShipmentId { get; set; }
    public Guid VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
