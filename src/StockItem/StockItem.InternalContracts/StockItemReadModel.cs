namespace StockItem.InternalContracts;

public sealed class StockItemReadModel
{
    public Guid Id { get; init; }
    public Guid VariantId { get; init; }
    public Guid WarehouseId { get; init; }
    public int Total { get; init; }
    public int Reserved { get; init; }
    public int Available => Total - Reserved;
    public int LowStockThreshold { get; init; }
    public string Status { get; init; } = string.Empty;
    public IReadOnlyList<ReservationReadModel> Reservations { get; init; } = new List<ReservationReadModel>();
}

public sealed class ReservationReadModel
{
    public Guid OrderId { get; init; }
    public int Quantity { get; init; }
    public DateTime ReservedAt { get; init; }
}
