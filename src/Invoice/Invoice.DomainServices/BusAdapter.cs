namespace Invoice.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.Events;
using Invoice.Domain;
using Invoice.Domain.Abstraction;

public sealed class BusAdapter : IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    private readonly IEventBus _eventBus;

    public BusAdapter(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(AggregateResult<IInvoice, IInvoiceAnemicModel> value)
    {
        if (value.IsSuccess)
        {
            _eventBus.Publish(value);
        }
    }
}
