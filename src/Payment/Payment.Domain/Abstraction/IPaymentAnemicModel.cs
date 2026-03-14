namespace Payment.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IPaymentAnemicModel : IAnemicModel<IPayment>
{
    IPaymentRoot Root { get; }
    IReadOnlyList<ITransaction> Transactions { get; }
    decimal TotalRefunded { get; }
}
