namespace Promotion.InternalContracts;

public interface IPromotionQueryRepository
{
    Task<PromotionReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PromotionReadModel>> SearchAsync(
        string? name,
        string? discountType,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(
        string? name,
        string? discountType,
        string? status,
        CancellationToken cancellationToken = default);
}
