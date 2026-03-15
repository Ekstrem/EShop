namespace Payment.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Payment.Domain;
using Payment.Domain.Abstraction;
using Payment.InternalContracts;

public sealed class AggregateProvider
{
    private readonly IQueryRepository<IPayment, IPaymentAnemicModel> _repository;

    public AggregateProvider(IQueryRepository<IPayment, IPaymentAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<Notifier> LoadAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _repository.GetByIdAsync(id, ct);
        return Notifier.CreateInstance(model);
    }

    public Notifier CreateNew()
    {
        return Notifier.CreateEmpty();
    }
}
