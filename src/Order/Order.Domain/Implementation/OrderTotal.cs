namespace Order.Domain.Implementation;

using Order.Domain.Abstraction;

internal sealed class OrderTotal : IOrderTotal
{
    public decimal SubTotal { get; private set; }
    public decimal DiscountTotal { get; private set; }
    public decimal ShippingCost { get; private set; }
    public decimal Total { get; private set; }

    private OrderTotal() { }

    public static IOrderTotal CreateInstance(
        decimal subTotal,
        decimal discountTotal,
        decimal shippingCost,
        decimal total)
        => new OrderTotal
        {
            SubTotal = subTotal,
            DiscountTotal = discountTotal,
            ShippingCost = shippingCost,
            Total = total
        };
}
