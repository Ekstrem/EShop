namespace Customer.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

/// <summary>
/// Value object representing a customer address.
/// </summary>
public interface IAddress : IValueObject
{
    string Street { get; }
    string City { get; }
    string ZipCode { get; }
    string Country { get; }
    bool IsDefault { get; }
}
