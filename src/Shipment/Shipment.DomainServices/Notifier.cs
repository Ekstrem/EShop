using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Shipment.Domain;
using Shipment.Domain.Abstraction;

namespace Shipment.DomainServices;

/// <summary>
/// Handles notifications for the Shipment context domain events.
/// </summary>
public sealed class Notifier : INotifier<IShipment>
{
    public void Notify<TModel>(AggregateResult<IShipment, TModel> result)
        where TModel : IAnemicModel<IShipment>
    {
        // Send notifications based on domain events.
    }
}
