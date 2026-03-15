namespace Order.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IOrderTotal : IValueObject
{
    decimal SubTotal { get; }
    decimal DiscountTotal { get; }
    decimal ShippingCost { get; }
    decimal Total { get; }
}
