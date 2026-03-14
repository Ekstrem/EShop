namespace StockItem.Domain.Implementation;

using StockItem.Domain.Abstraction;

internal sealed class Reservation : IReservation
{
    public Guid OrderId { get; private set; }
    public int Quantity { get; private set; }
    public DateTime ReservedAt { get; private set; }

    private Reservation() { }

    public static IReservation CreateInstance(Guid orderId, int quantity, DateTime reservedAt)
        => new Reservation
        {
            OrderId = orderId,
            Quantity = quantity,
            ReservedAt = reservedAt
        };
}
