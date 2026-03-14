using Hive.SeedWorks.TacticalPatterns;

namespace Shipment.Domain.Abstraction;

/// <summary>
/// Value object representing an item within a shipment.
/// </summary>
public interface IShipmentItem : IValueObject
{
    Guid VariantId { get; }
    string ProductName { get; }
    int Quantity { get; }
}
