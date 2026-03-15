namespace Customer.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

/// <summary>
/// Value object representing a customer consent record.
/// </summary>
public interface IConsent : IValueObject
{
    string ConsentType { get; }
    bool IsGranted { get; }
    DateTime GrantedAt { get; }
}
