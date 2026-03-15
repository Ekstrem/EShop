namespace Order.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IOrderRoot : IValueObject
{
    Guid CustomerId { get; }
    string OrderNumber { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
    string ShippingAddress { get; }
}
