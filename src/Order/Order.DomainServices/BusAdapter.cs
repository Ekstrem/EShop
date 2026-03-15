namespace Order.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using Order.Domain;
using Order.Domain.Abstraction;

public class BusAdapter : IObserver<AggregateResult<IOrder, IOrderAnemicModel>>
{
    public BusAdapter() { }

    public void OnCompleted() { }

    public void OnError(Exception error)
        => throw error;

    public void OnNext(AggregateResult<IOrder, IOrderAnemicModel> value)
    {
        // Publish domain events to event bus (stub).
    }
}
