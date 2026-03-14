namespace StockItem.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IStockItemRoot : IAggregateRoot<IStockItem>
{
    Guid VariantId { get; }
    Guid WarehouseId { get; }
    int Total { get; }
    int Reserved { get; }
    int LowStockThreshold { get; }
    string Status { get; }
}
