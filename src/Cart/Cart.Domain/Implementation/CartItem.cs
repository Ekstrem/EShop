namespace Cart.Domain.Implementation;

using Cart.Domain.Abstraction;

internal sealed class CartItem : ICartItem
{
    public Guid VariantId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private CartItem() { }

    public static ICartItem CreateInstance(
        Guid variantId,
        string productName,
        string sku,
        int quantity,
        decimal unitPrice)
        => new CartItem
        {
            VariantId = variantId,
            ProductName = productName,
            Sku = sku,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
}
