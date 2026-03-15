namespace Review.Storage;

using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Review.Domain;
using Review.Domain.Abstraction;

/// <summary>
/// Command-side database context for the Review bounded context.
/// </summary>
public sealed class CommandDbContext : ICommandDbContext<IReview>
{
    public Task SaveAsync(IAnemicModel<IReview> model, CancellationToken cancellationToken = default)
    {
        // Persist aggregate state to command store (PostgreSQL, etc.)
        return Task.CompletedTask;
    }
}
