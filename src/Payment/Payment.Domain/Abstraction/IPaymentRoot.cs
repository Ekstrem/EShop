namespace Payment.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IPaymentRoot : IAggregateRoot<IPayment>
{
    Guid OrderId { get; }
    decimal Amount { get; }
    string Currency { get; }
    string PaymentMethod { get; }
    string Status { get; }
}
