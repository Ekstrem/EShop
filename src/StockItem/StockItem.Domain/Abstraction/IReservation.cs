namespace StockItem.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IReservation : IValueObject
{
    Guid OrderId { get; }
    int Quantity { get; }
    DateTime ReservedAt { get; }
}
