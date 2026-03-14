namespace Order.Domain.Implementation;

using Order.Domain.Abstraction;

internal sealed class OrderRoot : IOrderRoot
{
    public Guid CustomerId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Placed";
    public DateTime CreatedAt { get; private set; }
    public string ShippingAddress { get; private set; } = string.Empty;

    private OrderRoot() { }

    public static IOrderRoot CreateInstance(
        Guid customerId,
        string orderNumber,
        string status,
        DateTime createdAt,
        string shippingAddress)
        => new OrderRoot
        {
            CustomerId = customerId,
            OrderNumber = orderNumber,
            Status = status,
            CreatedAt = createdAt,
            ShippingAddress = shippingAddress
        };
}
