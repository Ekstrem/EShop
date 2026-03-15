using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Session.Domain;
using Session.Domain.Abstraction;

namespace Session.DomainServices;

/// <summary>
/// Observes aggregate results and publishes domain events to the event bus.
/// </summary>
public sealed class BusAdapter : IObserver<AggregateResult<ISession, ISessionAnemicModel>>
{
    private readonly IEventBus _eventBus;

    public BusAdapter(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void OnNext(AggregateResult<ISession, ISessionAnemicModel> value)
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
