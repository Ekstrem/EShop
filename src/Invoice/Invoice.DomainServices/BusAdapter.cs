namespace Invoice.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using Invoice.Domain;
using Invoice.Domain.Abstraction;

public sealed class BusAdapter : IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    public BusAdapter() { }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(AggregateResult<IInvoice, IInvoiceAnemicModel> value)
    {
        if (value.IsSuccess())
        {
            // Publish domain events to event bus (stub).
        }
    }
}
