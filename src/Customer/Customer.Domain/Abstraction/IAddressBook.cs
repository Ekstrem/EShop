using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Abstraction;

/// <summary>
/// Value object representing the customer's address book.
/// </summary>
public interface IAddressBook : IValueObject
{
    IReadOnlyList<IAddress> Addresses { get; }
}
