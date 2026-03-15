using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Shipment.Domain;
using Shipment.Domain.Abstraction;

namespace Shipment.DomainServices;

/// <summary>
/// Adapts domain events to the event bus for the Shipment context.
/// </summary>
public sealed class BusAdapter
{
    public Task PublishAsync(
        AggregateResult<IShipment, IShipmentAnemicModel> result,
        CancellationToken cancellationToken = default)
    {
        // Publish domain events to the event bus.
        return Task.CompletedTask;
    }
}
