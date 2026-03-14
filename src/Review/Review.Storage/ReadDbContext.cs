namespace Review.Storage;

using Review.InternalContracts;

/// <summary>
/// Read-side database context for the Review bounded context.
/// </summary>
public sealed class ReadDbContext : IReviewQueryRepository
{
    public Task<ReviewReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Query read model from read store (PostgreSQL, etc.)
        return Task.FromResult<ReviewReadModel?>(null);
    }

    public Task<ReviewReadModel?> GetByCustomerAndProductAsync(
        Guid customerId,
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        // Query read model by customer and product
        return Task.FromResult<ReviewReadModel?>(null);
    }

    public Task<IReadOnlyList<ReviewReadModel>> GetByProductIdAsync(
        Guid productId,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        // Query read models by product
        return Task.FromResult<IReadOnlyList<ReviewReadModel>>(new List<ReviewReadModel>());
    }

    public Task<int> CountByProductIdAsync(
        Guid productId,
        string? status,
        CancellationToken cancellationToken = default)
    {
        // Count reviews by product
        return Task.FromResult(0);
    }
}
