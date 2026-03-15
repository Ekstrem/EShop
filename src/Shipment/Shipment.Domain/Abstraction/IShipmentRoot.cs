using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace Shipment.Domain.Abstraction;

/// <summary>
/// Aggregate root for the Shipment bounded context.
/// </summary>
public interface IShipmentRoot : IValueObject
{
    Guid Id { get; }
    Guid OrderId { get; }
    string TrackingNumber { get; }
    string Carrier { get; }
    string ShippingAddress { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
}
