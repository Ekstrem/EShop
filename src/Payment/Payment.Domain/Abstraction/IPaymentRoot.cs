namespace Payment.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IPaymentRoot : IValueObject
{
    Guid OrderId { get; }
    decimal Amount { get; }
    string Currency { get; }
    string PaymentMethod { get; }
    string Status { get; }
}
