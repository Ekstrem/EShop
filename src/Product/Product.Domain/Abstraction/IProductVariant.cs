namespace Product.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IProductVariant : IValueObject
{
    string Sku { get; }
    string Size { get; }
    string Color { get; }
    decimal Price { get; }
}
