namespace Product.InternalContracts;

public interface IProductQueryRepository
{
    Task<ProductReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductReadModel>> SearchAsync(
        string? name,
        Guid? categoryId,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(
        string? name,
        Guid? categoryId,
        string? status,
        CancellationToken cancellationToken = default);
}
