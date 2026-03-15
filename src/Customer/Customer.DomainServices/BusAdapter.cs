using Hive.SeedWorks.Events;
using Customer.Domain;
using Customer.Domain.Abstraction;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.DomainServices;

/// <summary>
/// Observes aggregate results and publishes domain events to the event bus.
/// </summary>
public sealed class BusAdapter : IObserver<AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    private readonly IEventBus _eventBus;

    public BusAdapter(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void OnNext(AggregateResult<ICustomer, ICustomerAnemicModel> value)
    {
        _eventBus.Publish(value);
    }

    public void OnError(Exception error)
    {
        // Log and handle event bus errors.
    }

    public void OnCompleted()
    {
        // No-op on stream completion.
    }
}
