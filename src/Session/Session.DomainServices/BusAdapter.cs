using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using Session.Domain;
using Session.Domain.Abstraction;

namespace Session.DomainServices;

/// <summary>
/// Observes aggregate results and publishes domain events to the event bus.
/// </summary>
public sealed class BusAdapter : IObserver<AggregateResult<ISession, ISessionAnemicModel>>
{
    public BusAdapter() { }

    public void OnNext(AggregateResult<ISession, ISessionAnemicModel> value)
    {
        // Publish domain events to event bus (stub).
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
