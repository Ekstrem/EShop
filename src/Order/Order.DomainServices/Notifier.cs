namespace Order.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain;
using Order.Domain.Abstraction;

public class Notifier : IObserver<AggregateResult<IOrder, IOrderAnemicModel>>
{
    private readonly IObserver<AggregateResult<IOrder, IOrderAnemicModel>> _inner;

    public Notifier(IObserver<AggregateResult<IOrder, IOrderAnemicModel>> inner)
    {
        _inner = inner;
    }

    public void OnCompleted()
        => _inner.OnCompleted();

    public void OnError(Exception error)
        => _inner.OnError(error);

    public void OnNext(AggregateResult<IOrder, IOrderAnemicModel> value)
    {
        // Decoration point for logging, metrics, etc.
        _inner.OnNext(value);
    }
}
