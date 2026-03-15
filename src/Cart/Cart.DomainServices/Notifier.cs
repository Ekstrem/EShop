namespace Cart.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;

public class Notifier : INotifier<ICart>, IObserver<AggregateResult<ICart, ICartAnemicModel>>
{
    private readonly IObserver<AggregateResult<ICart, ICartAnemicModel>> _inner;

    public Notifier(IObserver<AggregateResult<ICart, ICartAnemicModel>> inner)
    {
        _inner = inner;
    }

    public void Notify<TModel>(AggregateResult<ICart, TModel> result)
        where TModel : IAnemicModel<ICart>
    {
    }

    public void OnCompleted()
        => _inner.OnCompleted();

    public void OnError(Exception error)
        => _inner.OnError(error);

    public void OnNext(AggregateResult<ICart, ICartAnemicModel> value)
    {
        // Decoration point for logging, metrics, etc.
        _inner.OnNext(value);
    }
}
