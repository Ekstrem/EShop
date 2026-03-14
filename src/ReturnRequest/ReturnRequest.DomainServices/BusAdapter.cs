using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.DomainServices;

/// <summary>
/// Adapts domain events to the event bus for the ReturnRequest context.
/// </summary>
public sealed class BusAdapter
{
    public Task PublishAsync(
        AggregateResult<IReturnRequest, IReturnRequestAnemicModel> result,
        CancellationToken cancellationToken = default)
    {
        // Publish domain events to the event bus.
        return Task.CompletedTask;
    }
}
