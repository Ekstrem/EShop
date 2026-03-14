using Hive.SeedWorks.TacticalPatterns;
using Session.Domain;
using Session.Domain.Abstraction;

namespace Session.DomainServices;

/// <summary>
/// Decorator that notifies observers after aggregate operations complete.
/// </summary>
public sealed class Notifier
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

    public async Task HandleAsync(
        AggregateResult<ISession, ISessionAnemicModel> result,
        CancellationToken ct = default)
    {
        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);
    }
}
