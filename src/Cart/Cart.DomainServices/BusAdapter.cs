using Hive.SeedWorks.Events;
namespace Cart.DomainServices;

using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;

public class BusAdapter : IObserver<AggregateResult<ICart, ICartAnemicModel>>
{
    private readonly IEventBus _eventBus;

    public BusAdapter(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void OnCompleted() { }

    public void OnError(Exception error)
        => throw error;

    public void OnNext(AggregateResult<ICart, ICartAnemicModel> value)
        => _eventBus.Publish(value);
}
