namespace Payment.Domain.Specifications;

using Payment.Domain.Abstraction;

internal sealed class TransactionDeduplicationValidator
{
    public bool IsSatisfiedBy(IPaymentAnemicModel model, string providerTransactionId)
        => !model.Transactions.Any(t => t.ProviderTransactionId == providerTransactionId);

    public string Reason => "Transaction with this provider ID has already been processed.";
}
