using Customer.Domain;
using Customer.Domain.Abstraction;
using Customer.Domain.Implementation;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;

namespace Customer.DomainServices;

/// <summary>
/// Provides aggregate reconstitution and persistence orchestration for the Customer context.
/// </summary>
public sealed class AggregateProvider
{
    private readonly IRepository<ICustomer, ICustomerAnemicModel> _repository;

    public AggregateProvider(IRepository<ICustomer, ICustomerAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<ICustomerAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _repository.GetByIdAsync(id, ct);
    }

    public async Task SaveAsync(
        AggregateResult<ICustomer, ICustomerAnemicModel> result,
        CancellationToken ct = default)
    {
        await _repository.SaveAsync(result, ct);
    }
}
