namespace Payment.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface ITransaction : IValueObject
{
    string ProviderTransactionId { get; }
    string Type { get; }
    decimal Amount { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
}
