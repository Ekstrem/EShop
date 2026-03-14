namespace Cart.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
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
