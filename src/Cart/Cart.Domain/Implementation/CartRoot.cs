namespace Cart.Domain.Implementation;

using Cart.Domain.Abstraction;

internal sealed class CartRoot : ICartRoot
{
    public Guid CustomerId { get; private set; }
    public Guid SessionId { get; private set; }
    public string Status { get; private set; } = "Active";
    public DateTime CreatedAt { get; private set; }

    private CartRoot() { }

    public static ICartRoot CreateInstance(
        Guid customerId,
        Guid sessionId,
        string status = "Active",
        DateTime? createdAt = null)
        => new CartRoot
        {
            CustomerId = customerId,
            SessionId = sessionId,
            Status = status,
            CreatedAt = createdAt ?? DateTime.UtcNow
        };
}
