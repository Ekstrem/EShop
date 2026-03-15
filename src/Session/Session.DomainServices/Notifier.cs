using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Session.Domain;
using Session.Domain.Abstraction;

namespace Session.DomainServices;

/// <summary>
/// Decorator that notifies observers after aggregate operations complete.
/// </summary>
public sealed class Notifier : INotifier<ISession>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<ISession, ISessionAnemicModel>> _observer;

    public Notifier(
        AggregateProvider provider,
        IObserver<AggregateResult<ISession, ISessionAnemicModel>> observer)
    {
        _provider = provider;
        _observer = observer;
    }

    public void Notify<TModel>(AggregateResult<ISession, TModel> result)
        where TModel : IAnemicModel<ISession>
    {
    }

    public async Task HandleAsync(
        AggregateResult<ISession, ISessionAnemicModel> result,
        CancellationToken ct = default)
    {
        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);
    }
}
