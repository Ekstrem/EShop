namespace Payment.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface ITransaction : IValueObject
{
    string ProviderTransactionId { get; }
    string Type { get; }
    decimal Amount { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
}
