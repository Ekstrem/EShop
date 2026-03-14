using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Implementation;

/// <summary>
/// Immutable implementation of the ReturnRequest aggregate root.
/// </summary>
internal sealed class ReturnRequestRoot : IReturnRequestRoot
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string RmaNumber { get; private set; } = string.Empty;
    public string Reason { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Requested";
    public DateTime RequestedAt { get; private set; }

    private ReturnRequestRoot() { }

    public static IReturnRequestRoot CreateInstance(
        Guid id,
        Guid orderId,
        Guid customerId,
        string rmaNumber,
        string reason,
        string status,
        DateTime requestedAt)
        => new ReturnRequestRoot
        {
            Id = id,
            OrderId = orderId,
            CustomerId = customerId,
            RmaNumber = rmaNumber,
            Reason = reason,
            Status = status,
            RequestedAt = requestedAt
        };
}
