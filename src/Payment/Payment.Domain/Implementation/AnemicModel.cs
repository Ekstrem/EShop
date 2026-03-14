namespace Payment.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Payment.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<IPayment>, IPaymentAnemicModel
{
    public IPaymentRoot Root { get; internal set; } = null!;
    public IReadOnlyList<ITransaction> Transactions { get; internal set; } = new List<ITransaction>();
    public decimal TotalRefunded => Transactions
        .Where(t => t.Type == "Refund" && t.Status == "Completed")
        .Sum(t => t.Amount);
}
