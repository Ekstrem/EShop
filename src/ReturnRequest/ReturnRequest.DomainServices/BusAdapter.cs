using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
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
