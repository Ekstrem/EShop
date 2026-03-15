using Hive.SeedWorks.TacticalPatterns;

namespace Shipment.Domain;

/// <summary>
/// Bounded context marker for the Shipment context.
/// </summary>
public interface IShipment : IBoundedContext
{
}

/// <summary>
/// Describes the Shipment bounded context.
/// </summary>
public sealed class ShipmentBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Shipment";
    public int MicroserviceVersion => 1;
}
