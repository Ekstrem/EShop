namespace StockItem.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using StockItem.Domain.Abstraction;
using StockItem.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public IStockItemAnemicModel Model { get; }

    private Aggregate(IStockItemAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(IStockItemAnemicModel model) => new(model);

    private AggregateResult<IStockItem, IStockItemAnemicModel> Success(IStockItemAnemicModel newModel)
    {
        var data = BusinessOperationData<IStockItem, IStockItemAnemicModel>
            .Commit<IStockItem, IStockItemAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<IStockItem, IStockItemAnemicModel>(data);
    }

    private AggregateResult<IStockItem, IStockItemAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<IStockItem, IStockItemAnemicModel>
            .Commit<IStockItem, IStockItemAnemicModel>(Model, Model);
        return new AggregateResultException<IStockItem, IStockItemAnemicModel>(
            data, new FailedSpecification<IStockItem, IStockItemAnemicModel>(error));
    }

    public AggregateResult<IStockItem, IStockItemAnemicModel> ReserveStock(
        Guid orderId,
        int quantity)
    {
        var uniqueOrderValidator = new UniqueOrderReservationValidator();
        if (!uniqueOrderValidator.IsSatisfiedBy(Model, orderId))
            return Fail($"Reservation for order '{orderId}' already exists on this stock item.");

        var available = Model.Root.Total - Model.Root.Reserved;

        var availableValidator = new AvailableStockValidator();
        if (!availableValidator.IsSatisfiedBy(available, quantity))
            return Fail($"Insufficient stock. Available: {available}, Requested: {quantity}.");

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

        return Success(anemic);
    }

    public AggregateResult<IStockItem, IStockItemAnemicModel> ReleaseStock(Guid orderId)
    {
        var reservation = Model.Reservations.FirstOrDefault(r => r.OrderId == orderId);
        if (reservation is null)
            return Fail($"No reservation found for order '{orderId}'.");

        var newReserved = Model.Root.Reserved - reservation.Quantity;

        var nonNegativeValidator = new NonNegativeReservedValidator();
        if (!nonNegativeValidator.IsSatisfiedBy(newReserved))
            return Fail("Reserved quantity cannot be negative.");

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

        return Success(anemic);
    }

    public AggregateResult<IStockItem, IStockItemAnemicModel> ReplenishStock(int quantity)
    {
        if (quantity <= 0)
            return Fail("Replenish quantity must be greater than zero.");

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

        return Success(anemic);
    }

    public AggregateResult<IStockItem, IStockItemAnemicModel> AdjustStock(int newTotal)
    {
        var nonNegativeTotalValidator = new NonNegativeTotalValidator();
        if (!nonNegativeTotalValidator.IsSatisfiedBy(newTotal))
            return Fail("Total stock cannot be negative.");

        if (newTotal < Model.Root.Reserved)
            return Fail($"Cannot adjust total below reserved quantity. Reserved: {Model.Root.Reserved}.");

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

        return Success(anemic);
    }
}
