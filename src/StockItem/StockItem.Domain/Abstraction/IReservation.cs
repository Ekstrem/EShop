namespace StockItem.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IReservation : IValueObject
{
    Guid OrderId { get; }
    int Quantity { get; }
    DateTime ReservedAt { get; }
}
