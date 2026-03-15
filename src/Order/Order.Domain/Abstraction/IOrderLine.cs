namespace Order.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IOrderLine : IValueObject
{
    Guid VariantId { get; }
    string ProductName { get; }
    string Sku { get; }
    int Quantity { get; }
    decimal UnitPrice { get; }
    decimal Discount { get; }
}
