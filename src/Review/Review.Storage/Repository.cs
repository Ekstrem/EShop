namespace Review.Storage;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Review.Domain;
using Review.Domain.Abstraction;

/// <summary>
/// Repository implementation for the Review aggregate.
/// </summary>
public sealed class Repository : IRepository<IReview, IReviewAnemicModel>
{
    public Task<IReviewAnemicModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Load aggregate from command store
        return Task.FromResult<IReviewAnemicModel?>(null);
    }

    public Task SaveAsync(AggregateResult<IReview, IReviewAnemicModel> result, CancellationToken cancellationToken = default)
    {
        // Persist aggregate to command store
        return Task.CompletedTask;
    }
}
