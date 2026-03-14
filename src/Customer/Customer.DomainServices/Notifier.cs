using Customer.Domain;
using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.DomainServices;

/// <summary>
/// Decorator that notifies observers after aggregate operations complete.
/// </summary>
public sealed class Notifier
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<ICustomer, ICustomerAnemicModel>> _observer;

    public Notifier(
        AggregateProvider provider,
        IObserver<AggregateResult<ICustomer, ICustomerAnemicModel>> observer)
    {
        _provider = provider;
        _observer = observer;
    }

    public async Task HandleAsync(
        AggregateResult<ICustomer, ICustomerAnemicModel> result,
        CancellationToken ct = default)
    {
        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);
    }
}
