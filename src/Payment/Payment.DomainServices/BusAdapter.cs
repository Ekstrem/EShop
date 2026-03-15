namespace Payment.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using Payment.Domain;
using Payment.Domain.Abstraction;

public sealed class BusAdapter : IObserver<AggregateResult<IPayment, IPaymentAnemicModel>>
{
    public BusAdapter() { }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(AggregateResult<IPayment, IPaymentAnemicModel> value)
    {
        if (value.IsSuccess())
        {
            // Publish domain events to event bus (stub).
        }
    }
}
