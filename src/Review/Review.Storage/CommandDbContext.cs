namespace Review.Storage;

using Hive.SeedWorks.TacticalPatterns;
using Review.Domain;
using Review.Domain.Abstraction;

/// <summary>
/// Command-side database context for the Review bounded context.
/// </summary>
public sealed class CommandDbContext : ICommandDbContext<IReview>
{
    public Task SaveAsync(IReviewAnemicModel model, CancellationToken cancellationToken = default)
    {
        // Persist aggregate state to command store (PostgreSQL, etc.)
        return Task.CompletedTask;
    }
}
