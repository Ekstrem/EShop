namespace Cart.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface ICartItem : IValueObject
{
    Guid VariantId { get; }
    string ProductName { get; }
    string Sku { get; }
    int Quantity { get; }
    decimal UnitPrice { get; }
}
