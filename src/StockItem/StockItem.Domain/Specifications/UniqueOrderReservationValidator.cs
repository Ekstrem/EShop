namespace StockItem.Domain.Specifications;

using StockItem.Domain.Abstraction;

internal sealed class UniqueOrderReservationValidator
{
    public bool IsSatisfiedBy(IStockItemAnemicModel model, Guid orderId)
        => !model.Reservations.Any(r => r.OrderId == orderId);

    public string Reason => "Only one reservation per order is allowed per stock item.";
}
