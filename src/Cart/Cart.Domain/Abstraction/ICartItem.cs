namespace Cart.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface ICartItem : IValueObject
{
    Guid VariantId { get; }
    string ProductName { get; }
    string Sku { get; }
    int Quantity { get; }
    decimal UnitPrice { get; }
}
