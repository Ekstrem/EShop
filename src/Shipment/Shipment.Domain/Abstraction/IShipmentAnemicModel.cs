using Hive.SeedWorks.TacticalPatterns;

namespace Shipment.Domain.Abstraction;

/// <summary>
/// Anemic model contract for the Shipment aggregate.
/// </summary>
public interface IShipmentAnemicModel : IAnemicModel<IShipment>
{
    IShipmentRoot Root { get; }
    IReadOnlyList<IShipmentItem> Items { get; }
    IShippingLabel? Label { get; }
}
