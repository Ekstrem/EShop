namespace AggregateRating.Storage;

using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;

/// <summary>
/// Command-side database context for the AggregateRating bounded context.
/// </summary>
public sealed class CommandDbContext : ICommandDbContext<IAggregateRating>
{
    public Task SaveAsync(IAnemicModel<IAggregateRating> model, CancellationToken cancellationToken = default)
    {
        // Persist aggregate state to command store (PostgreSQL, etc.)
        return Task.CompletedTask;
    }
}
