using Hive.SeedWorks.TacticalPatterns;
using Session.Domain;
using Session.Domain.Abstraction;

namespace Session.DomainServices;

/// <summary>
/// Provides aggregate reconstitution and persistence orchestration for the Session context.
/// </summary>
public sealed class AggregateProvider
{
    private readonly IRepository<ISession, ISessionAnemicModel> _repository;

    public AggregateProvider(IRepository<ISession, ISessionAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<ISessionAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _repository.GetByIdAsync(id, ct);
    }

    public async Task SaveAsync(
        AggregateResult<ISession, ISessionAnemicModel> result,
        CancellationToken ct = default)
    {
        await _repository.SaveAsync(result.AnemicModel, ct);
    }
}
