namespace Payment.Domain.Implementation;

using Payment.Domain.Abstraction;

internal sealed class PaymentRoot : IPaymentRoot
{
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public string PaymentMethod { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Initiated";

    private PaymentRoot() { }

    public static IPaymentRoot CreateInstance(
        Guid orderId,
        decimal amount,
        string currency,
        string paymentMethod,
        string status = "Initiated")
        => new PaymentRoot
        {
            OrderId = orderId,
            Amount = amount,
            Currency = currency,
            PaymentMethod = paymentMethod,
            Status = status
        };
}
