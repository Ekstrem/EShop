namespace StockItem.Storage;

using Microsoft.EntityFrameworkCore;
using StockItem.InternalContracts;

public sealed class StockItemRepository : IStockItemQueryRepository
{
    private readonly StockItemReadDbContext _context;

    public StockItemRepository(StockItemReadDbContext context)
    {
        _context = context;
    }

    public async Task<StockItemReadModel?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.StockItems
            .Include(s => s.Reservations)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<StockItemReadModel?> GetByVariantAndWarehouseAsync(
        Guid variantId,
        Guid warehouseId,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.StockItems
            .Include(s => s.Reservations)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                s => s.VariantId == variantId && s.WarehouseId == warehouseId,
                cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<StockItemReadModel>> GetByVariantIdAsync(
        Guid variantId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.StockItems
            .Include(s => s.Reservations)
            .AsNoTracking()
            .Where(s => s.VariantId == variantId)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<IReadOnlyList<StockItemReadModel>> GetLowStockItemsAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.StockItems
            .Include(s => s.Reservations)
            .AsNoTracking()
            .Where(s => s.Status == "LowStock")
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<IReadOnlyList<StockItemReadModel>> GetOutOfStockItemsAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.StockItems
            .Include(s => s.Reservations)
            .AsNoTracking()
            .Where(s => s.Status == "OutOfStock")
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    private static StockItemReadModel MapToReadModel(StockItemEntity entity)
        => new()
        {
            Id = entity.Id,
            VariantId = entity.VariantId,
            WarehouseId = entity.WarehouseId,
            Total = entity.Total,
            Reserved = entity.Reserved,
            LowStockThreshold = entity.LowStockThreshold,
            Status = entity.Status,
            Reservations = entity.Reservations.Select(r => new ReservationReadModel
            {
                OrderId = r.OrderId,
                Quantity = r.Quantity,
                ReservedAt = r.ReservedAt
            }).ToList()
        };
}
