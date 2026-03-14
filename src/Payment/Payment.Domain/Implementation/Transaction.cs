namespace Payment.Domain.Implementation;

using Payment.Domain.Abstraction;

internal sealed class Transaction : ITransaction
{
    public string ProviderTransactionId { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Transaction() { }

    public static ITransaction CreateInstance(
        string providerTransactionId,
        string type,
        decimal amount,
        string status,
        DateTime createdAt)
        => new Transaction
        {
            ProviderTransactionId = providerTransactionId,
            Type = type,
            Amount = amount,
            Status = status,
            CreatedAt = createdAt
        };
}
