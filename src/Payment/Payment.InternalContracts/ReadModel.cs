namespace Payment.InternalContracts;

public sealed class PaymentReadModel
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalRefunded { get; set; }
    public IReadOnlyList<TransactionReadModel> Transactions { get; set; } = new List<TransactionReadModel>();
}

public sealed class TransactionReadModel
{
    public string ProviderTransactionId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
