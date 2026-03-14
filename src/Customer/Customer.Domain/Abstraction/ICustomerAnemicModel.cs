using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Abstraction;

/// <summary>
/// Anemic model contract for the Customer aggregate.
/// </summary>
public interface ICustomerAnemicModel : IAnemicModel<ICustomer>
{
    ICustomerRoot Root { get; }
    IAddressBook AddressBook { get; }
    IReadOnlyList<IConsent> Consents { get; }
}
