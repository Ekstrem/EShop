namespace StockItem.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using StockItem.Domain.Abstraction;
using StockItem.Domain.Specifications;

internal sealed class Aggregate : Aggregate<IStockItem, IStockItemAnemicModel>
{
    private Aggregate(IStockItemAnemicModel model) : base(model) { }

    public static Aggregate CreateInstance(IStockItemAnemicModel model) => new(model);

    public AggregateResult<IStockItem, IStockItemAnemicModel> ReserveStock(
        Guid orderId,
        int quantity)
    {
        var uniqueOrderValidator = new UniqueOrderReservationValidator();
        if (!uniqueOrderValidator.IsSatisfiedBy(Model, orderId))
            return AggregateResult<IStockItem, IStockItemAnemicModel>.Fail(
                $"Reservation for order '{orderId}' already exists on this stock item.");

        var available = Model.Root.Total - Model.Root.Reserved;

        var availableValidator = new AvailableStockValidator();
        if (!availableValidator.IsSatisfiedBy(available, quantity))
            return AggregateResult<IStockItem, IStockItemAnemicModel>.Fail(
                $"Insufficient stock. Available: {available}, Requested: {quantity}.");

        var newReserved = Model.Root.Reserved + quantity;
        var root = StockItemRoot.CreateInstance(
            Model.Root.VariantId,
            Model.Root.WarehouseId,
            Model.Root.Total,
            newReserved,
            Model.Root.LowStockThreshold);

        var reservations = Model.Reservations.ToList();
        reservations.Add(Reservation.CreateInstance(orderId, quantity, DateTime.UtcNow));

        var anemic = new AnemicModel
        {
            Root = root,
            Reservations = reservations
        };

        return AggregateResult<IStockItem, IStockItemAnemicModel>.Ok(anemic);
    }

    public AggregateResult<IStockItem, IStockItemAnemicModel> ReleaseStock(Guid orderId)
    {
        var reservation = Model.Reservations.FirstOrDefault(r => r.OrderId == orderId);
        if (reservation is null)
            return AggregateResult<IStockItem, IStockItemAnemicModel>.Fail(
                $"No reservation found for order '{orderId}'.");

        var newReserved = Model.Root.Reserved - reservation.Quantity;

        var nonNegativeValidator = new NonNegativeReservedValidator();
        if (!nonNegativeValidator.IsSatisfiedBy(newReserved))
            return AggregateResult<IStockItem, IStockItemAnemicModel>.Fail(
                "Reserved quantity cannot be negative.");

        var root = StockItemRoot.CreateInstance(
            Model.Root.VariantId,
            Model.Root.WarehouseId,
            Model.Root.Total,
            newReserved,
            Model.Root.LowStockThreshold);

        var reservations = Model.Reservations.Where(r => r.OrderId != orderId).ToList();

        var anemic = new AnemicModel
        {
            Root = root,
            Reservations = reservations
        };

        return AggregateResult<IStockItem, IStockItemAnemicModel>.Ok(anemic);
    }

    public AggregateResult<IStockItem, IStockItemAnemicModel> ReplenishStock(int quantity)
    {
        if (quantity <= 0)
            return AggregateResult<IStockItem, IStockItemAnemicModel>.Fail(
                "Replenish quantity must be greater than zero.");

        var newTotal = Model.Root.Total + quantity;

        var root = StockItemRoot.CreateInstance(
            Model.Root.VariantId,
            Model.Root.WarehouseId,
            newTotal,
            Model.Root.Reserved,
            Model.Root.LowStockThreshold);

        var anemic = new AnemicModel
        {
            Root = root,
            Reservations = Model.Reservations.ToList()
        };

        return AggregateResult<IStockItem, IStockItemAnemicModel>.Ok(anemic);
    }

    public AggregateResult<IStockItem, IStockItemAnemicModel> AdjustStock(int newTotal)
    {
        var nonNegativeTotalValidator = new NonNegativeTotalValidator();
        if (!nonNegativeTotalValidator.IsSatisfiedBy(newTotal))
            return AggregateResult<IStockItem, IStockItemAnemicModel>.Fail(
                "Total stock cannot be negative.");

        if (newTotal < Model.Root.Reserved)
            return AggregateResult<IStockItem, IStockItemAnemicModel>.Fail(
                $"Cannot adjust total below reserved quantity. Reserved: {Model.Root.Reserved}.");

        var root = StockItemRoot.CreateInstance(
            Model.Root.VariantId,
            Model.Root.WarehouseId,
            newTotal,
            Model.Root.Reserved,
            Model.Root.LowStockThreshold);

        var anemic = new AnemicModel
        {
            Root = root,
            Reservations = Model.Reservations.ToList()
        };

        return AggregateResult<IStockItem, IStockItemAnemicModel>.Ok(anemic);
    }
}
