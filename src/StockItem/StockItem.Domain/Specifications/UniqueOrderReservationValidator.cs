namespace StockItem.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using StockItem.Domain.Abstraction;

internal sealed class UniqueOrderReservationValidator : IBusinessOperationValidator<IStockItemAnemicModel>
{
    public bool IsSatisfiedBy(IStockItemAnemicModel model, Guid orderId)
        => !model.Reservations.Any(r => r.OrderId == orderId);

    public bool IsSatisfiedBy(IStockItemAnemicModel model) => true;

    public string ErrorMessage => "Only one reservation per order is allowed per stock item.";
}
