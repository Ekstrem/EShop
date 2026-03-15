namespace Customer.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

/// <summary>
/// Value object representing the customer's address book.
/// </summary>
public interface IAddressBook : IValueObject
{
    IReadOnlyList<IAddress> Addresses { get; }
}
