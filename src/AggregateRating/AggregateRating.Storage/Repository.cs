namespace AggregateRating.Storage;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;

/// <summary>
/// Repository implementation for the AggregateRating aggregate.
/// </summary>
public sealed class Repository : IRepository<IAggregateRating, IAggregateRatingAnemicModel>
{
    public Task<IAggregateRatingAnemicModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Load aggregate from command store
        return Task.FromResult<IAggregateRatingAnemicModel?>(null);
    }

    public Task SaveAsync(AggregateResult<IAggregateRating, IAggregateRatingAnemicModel> result, CancellationToken cancellationToken = default)
    {
        // Persist aggregate to command store
        return Task.CompletedTask;
    }
}
