namespace Order.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Order.Domain;
using Order.Domain.Abstraction;

public class AggregateProvider
{
    private readonly IRepository<IOrder, IOrderAnemicModel> _repository;

    public AggregateProvider(IRepository<IOrder, IOrderAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<IOrderAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task SaveAsync(AggregateResult<IOrder, IOrderAnemicModel> result, CancellationToken ct = default)
        => await _repository.SaveAsync(result, ct);
}
