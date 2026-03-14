using Hive.SeedWorks.TacticalPatterns;
using Shipment.Domain;
using Shipment.Domain.Abstraction;

namespace Shipment.DomainServices;

/// <summary>
/// Handles notifications for the Shipment context domain events.
/// </summary>
public sealed class Notifier
{
    public Task NotifyAsync(
        AggregateResult<IShipment, IShipmentAnemicModel> result,
        CancellationToken cancellationToken = default)
    {
        // Send notifications based on domain events.
        return Task.CompletedTask;
    }
}
