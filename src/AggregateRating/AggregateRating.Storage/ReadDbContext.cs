namespace AggregateRating.Storage;

using AggregateRating.InternalContracts;

/// <summary>
/// Read-side database context for the AggregateRating bounded context.
/// </summary>
public sealed class ReadDbContext : IAggregateRatingQueryRepository
{
    public Task<AggregateRatingReadModel?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        // Query read model by product from read store (PostgreSQL, etc.)
        return Task.FromResult<AggregateRatingReadModel?>(null);
    }

    public Task<IReadOnlyList<AggregateRatingReadModel>> GetActiveRatingsAsync(
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        // Query active aggregate ratings from read store
        return Task.FromResult<IReadOnlyList<AggregateRatingReadModel>>(new List<AggregateRatingReadModel>());
    }
}
