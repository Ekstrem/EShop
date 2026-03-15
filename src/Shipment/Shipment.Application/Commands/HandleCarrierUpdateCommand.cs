using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;

namespace Shipment.Application.Commands;

/// <summary>
/// Command to handle a carrier status update (Shipped/InTransit -> InTransit/Delivered).
/// </summary>
public sealed class HandleCarrierUpdateCommand : IRequest<AggregateResult<IShipment, IShipmentAnemicModel>>
{
    public Guid ShipmentId { get; init; }
    public string NewStatus { get; init; } = string.Empty;
}
