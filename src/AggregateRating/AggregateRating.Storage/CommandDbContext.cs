namespace AggregateRating.Storage;

using Hive.SeedWorks.TacticalPatterns;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;

/// <summary>
/// Command-side database context for the AggregateRating bounded context.
/// </summary>
public sealed class CommandDbContext : ICommandDbContext<IAggregateRating>
{
    public Task SaveAsync(IAggregateRatingAnemicModel model, CancellationToken cancellationToken = default)
    {
        // Persist aggregate state to command store (PostgreSQL, etc.)
        return Task.CompletedTask;
    }
}
