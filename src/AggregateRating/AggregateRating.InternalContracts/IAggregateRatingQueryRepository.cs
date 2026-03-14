namespace AggregateRating.InternalContracts;

public interface IAggregateRatingQueryRepository
{
    Task<AggregateRatingReadModel?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AggregateRatingReadModel>> GetActiveRatingsAsync(
        int skip,
        int take,
        CancellationToken cancellationToken = default);
}
