namespace Order.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain;
using Order.Domain.Abstraction;

public class BusAdapter : IObserver<AggregateResult<IOrder, IOrderAnemicModel>>
{
    private readonly IEventBus _eventBus;

    public BusAdapter(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void OnCompleted() { }

    public void OnError(Exception error)
        => throw error;

    public void OnNext(AggregateResult<IOrder, IOrderAnemicModel> value)
        => _eventBus.Publish(value);
}
