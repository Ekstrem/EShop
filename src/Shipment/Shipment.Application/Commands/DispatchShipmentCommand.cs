using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;

namespace Shipment.Application.Commands;

/// <summary>
/// Command to dispatch a shipment (Packed -> Shipped).
/// Requires tracking number and label.
/// </summary>
public sealed class DispatchShipmentCommand : IRequest<AggregateResult<IShipment, IShipmentAnemicModel>>
{
    public Guid ShipmentId { get; init; }
    public string TrackingNumber { get; init; } = string.Empty;
    public string LabelUrl { get; init; } = string.Empty;
}
