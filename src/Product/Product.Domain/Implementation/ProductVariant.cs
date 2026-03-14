namespace Product.Domain.Implementation;

using Product.Domain.Abstraction;

internal sealed class ProductVariant : IProductVariant
{
    public string Sku { get; private set; } = string.Empty;
    public string Size { get; private set; } = string.Empty;
    public string Color { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    private ProductVariant() { }

    public static IProductVariant CreateInstance(
        string sku,
        string size,
        string color,
        decimal price)
        => new ProductVariant
        {
            Sku = sku,
            Size = size,
            Color = color,
            Price = price
        };
}
