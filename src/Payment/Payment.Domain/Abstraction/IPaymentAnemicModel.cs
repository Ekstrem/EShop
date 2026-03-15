namespace Payment.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IPaymentAnemicModel : IAnemicModel<IPayment>
{
    IPaymentRoot Root { get; }
    IReadOnlyList<ITransaction> Transactions { get; }
    decimal TotalRefunded { get; }
}
