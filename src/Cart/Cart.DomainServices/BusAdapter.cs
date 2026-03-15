namespace Cart.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using Cart.Domain;
using Cart.Domain.Abstraction;

public class BusAdapter : IObserver<AggregateResult<ICart, ICartAnemicModel>>
{
    public BusAdapter() { }

    public void OnCompleted() { }

    public void OnError(Exception error)
        => throw error;

    public void OnNext(AggregateResult<ICart, ICartAnemicModel> value)
    {
        // Publish domain events to event bus (stub).
    }
}
