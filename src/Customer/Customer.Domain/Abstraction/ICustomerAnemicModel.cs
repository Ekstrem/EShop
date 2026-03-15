namespace Customer.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

/// <summary>
/// Anemic model contract for the Customer aggregate.
/// </summary>
public interface ICustomerAnemicModel : IAnemicModel<ICustomer>
{
    ICustomerRoot Root { get; }
    IAddressBook AddressBook { get; }
    IReadOnlyList<IConsent> Consents { get; }
}
