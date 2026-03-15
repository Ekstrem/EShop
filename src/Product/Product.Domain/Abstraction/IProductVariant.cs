namespace Product.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IProductVariant : IValueObject
{
    string Sku { get; }
    string Size { get; }
    string Color { get; }
    decimal Price { get; }
}
