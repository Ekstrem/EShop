using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using Customer.Domain;
using Customer.Domain.Abstraction;

namespace Customer.DomainServices;

/// <summary>
/// Observes aggregate results and publishes domain events to the event bus.
/// </summary>
public sealed class BusAdapter : IObserver<AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    public BusAdapter() { }

    public void OnNext(AggregateResult<ICustomer, ICustomerAnemicModel> value)
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
