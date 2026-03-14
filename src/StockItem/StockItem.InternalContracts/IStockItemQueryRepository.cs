namespace StockItem.InternalContracts;

public interface IStockItemQueryRepository
{
    Task<StockItemReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StockItemReadModel?> GetByVariantAndWarehouseAsync(
        Guid variantId,
        Guid warehouseId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockItemReadModel>> GetByVariantIdAsync(
        Guid variantId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockItemReadModel>> GetLowStockItemsAsync(
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockItemReadModel>> GetOutOfStockItemsAsync(
        CancellationToken cancellationToken = default);
}
