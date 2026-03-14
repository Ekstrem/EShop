using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Abstraction;

/// <summary>
/// Value object representing a customer consent record.
/// </summary>
public interface IConsent : IValueObject
{
    string ConsentType { get; }
    bool IsGranted { get; }
    DateTime GrantedAt { get; }
}
