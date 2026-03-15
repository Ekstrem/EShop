namespace Order.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Order.Domain;
using Order.Domain.Abstraction;

public class Notifier : INotifier<IOrder>, IObserver<AggregateResult<IOrder, IOrderAnemicModel>>
{
    private readonly IObserver<AggregateResult<IOrder, IOrderAnemicModel>> _inner;

    public Notifier(IObserver<AggregateResult<IOrder, IOrderAnemicModel>> inner)
    {
        _inner = inner;
    }

    public void Notify<TModel>(AggregateResult<IOrder, TModel> result)
        where TModel : IAnemicModel<IOrder>
    {
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
