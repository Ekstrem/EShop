namespace DiscountCode.InternalContracts;

public interface IDiscountCodeQueryRepository
{
    Task<DiscountCodeReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DiscountCodeReadModel?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DiscountCodeReadModel>> SearchAsync(
        string? code,
        Guid? promotionId,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(
        string? code,
        Guid? promotionId,
        string? status,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
