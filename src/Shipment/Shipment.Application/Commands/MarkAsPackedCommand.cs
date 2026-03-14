using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;

namespace Shipment.Application.Commands;

/// <summary>
/// Command to mark a shipment as packed (Pending -> Packed).
/// </summary>
public sealed class MarkAsPackedCommand : IRequest<AggregateResult<IShipment, IShipmentAnemicModel>>
{
    public Guid ShipmentId { get; init; }
}
