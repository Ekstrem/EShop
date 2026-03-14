namespace Payment.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.Events;
using Payment.Domain;
using Payment.Domain.Abstraction;

public sealed class BusAdapter : IObserver<AggregateResult<IPayment, IPaymentAnemicModel>>
{
    private readonly IEventBus _eventBus;

    public BusAdapter(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(AggregateResult<IPayment, IPaymentAnemicModel> value)
    {
        if (value.IsSuccess)
        {
            _eventBus.Publish(value);
        }
    }
}
