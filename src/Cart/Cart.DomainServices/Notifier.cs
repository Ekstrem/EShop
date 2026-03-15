namespace Cart.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
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
