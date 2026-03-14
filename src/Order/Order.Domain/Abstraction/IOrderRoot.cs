namespace Order.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IOrderRoot : IAggregateRoot<IOrder>
{
    Guid CustomerId { get; }
    string OrderNumber { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
    string ShippingAddress { get; }
}
