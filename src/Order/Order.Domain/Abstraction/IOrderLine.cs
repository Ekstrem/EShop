namespace Order.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IOrderLine : IValueObject
{
    Guid VariantId { get; }
    string ProductName { get; }
    string Sku { get; }
    int Quantity { get; }
    decimal UnitPrice { get; }
    decimal Discount { get; }
}
