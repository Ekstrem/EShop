using Hive.SeedWorks.TacticalPatterns;
using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Implementation;

/// <summary>
/// Anemic model implementation for the Shipment aggregate.
/// </summary>
internal sealed class AnemicModel : AnemicModel<IShipment>, IShipmentAnemicModel
{
    public IShipmentRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IShipmentItem> Items { get; internal set; } = new List<IShipmentItem>();
    public IShippingLabel? Label { get; internal set; }
}
