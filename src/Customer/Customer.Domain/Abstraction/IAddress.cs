using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Abstraction;

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
