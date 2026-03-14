namespace StockItem.Domain.Implementation;

using StockItem.Domain.Abstraction;

internal sealed class StockItemRoot : IStockItemRoot
{
    public Guid VariantId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public int Total { get; private set; }
    public int Reserved { get; private set; }
    public int LowStockThreshold { get; private set; }
    public string Status { get; private set; } = "InStock";

    private StockItemRoot() { }

    public static IStockItemRoot CreateInstance(
        Guid variantId,
        Guid warehouseId,
        int total,
        int reserved,
        int lowStockThreshold,
        string? status = null)
    {
        var computedStatus = status ?? ComputeStatus(total, reserved, lowStockThreshold);
        return new StockItemRoot
        {
            VariantId = variantId,
            WarehouseId = warehouseId,
            Total = total,
            Reserved = reserved,
            LowStockThreshold = lowStockThreshold,
            Status = computedStatus
        };
    }

    private static string ComputeStatus(int total, int reserved, int lowStockThreshold)
    {
        var available = total - reserved;
        if (available <= 0) return "OutOfStock";
        if (available <= lowStockThreshold) return "LowStock";
        return "InStock";
    }
}
