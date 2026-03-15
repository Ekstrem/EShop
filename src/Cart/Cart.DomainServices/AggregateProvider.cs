namespace Cart.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Cart.Domain;
using Cart.Domain.Abstraction;

public class AggregateProvider
{
    private readonly IRepository<ICart, ICartAnemicModel> _repository;

    public AggregateProvider(IRepository<ICart, ICartAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<ICartAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task SaveAsync(AggregateResult<ICart, ICartAnemicModel> result, CancellationToken ct = default)
        => await _repository.SaveAsync(result, ct);
}
