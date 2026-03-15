namespace StockItem.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IStockItemRoot : IValueObject
{
    Guid VariantId { get; }
    Guid WarehouseId { get; }
    int Total { get; }
    int Reserved { get; }
    int LowStockThreshold { get; }
    string Status { get; }
}
