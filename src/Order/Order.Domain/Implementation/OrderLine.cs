namespace Order.Domain.Implementation;

using Order.Domain.Abstraction;

internal sealed class OrderLine : IOrderLine
{
    public Guid VariantId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }

    private OrderLine() { }

    public static IOrderLine CreateInstance(
        Guid variantId,
        string productName,
        string sku,
        int quantity,
        decimal unitPrice,
        decimal discount)
        => new OrderLine
        {
            VariantId = variantId,
            ProductName = productName,
            Sku = sku,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = discount
        };
}
