using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
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
