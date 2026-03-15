using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
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
