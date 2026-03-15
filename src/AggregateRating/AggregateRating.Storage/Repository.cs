namespace AggregateRating.Storage;

using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
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
