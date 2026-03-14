namespace Review.InternalContracts;

public interface IReviewQueryRepository
{
    Task<ReviewReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ReviewReadModel?> GetByCustomerAndProductAsync(
        Guid customerId,
        Guid productId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReviewReadModel>> GetByProductIdAsync(
        Guid productId,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
    Task<int> CountByProductIdAsync(
        Guid productId,
        string? status,
        CancellationToken cancellationToken = default);
}
