namespace DiscountCode.Domain.Implementation;

using DiscountCode.Domain.Abstraction;

internal sealed class Redemption : IRedemption
{
    public Guid OrderId { get; private set; }
    public DateTime RedeemedAt { get; private set; }

    private Redemption() { }

    public static IRedemption CreateInstance(Guid orderId, DateTime? redeemedAt = null)
        => new Redemption
        {
            OrderId = orderId,
            RedeemedAt = redeemedAt ?? DateTime.UtcNow
        };
}
