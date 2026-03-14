using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Implementation;

/// <summary>
/// Immutable anemic model for the Customer aggregate.
/// </summary>
public sealed class CustomerAnemicModel : AnemicModel<ICustomer>, ICustomerAnemicModel
{
    private CustomerAnemicModel(
        Guid id,
        ICustomerRoot root,
        IAddressBook addressBook,
        IReadOnlyList<IConsent> consents)
    {
        Id = id;
        Root = root;
        AddressBook = addressBook;
        Consents = consents;
    }

    public Guid Id { get; }
    public ICustomerRoot Root { get; }
    public IAddressBook AddressBook { get; }
    public IReadOnlyList<IConsent> Consents { get; }

    public static CustomerAnemicModel CreateInstance(
        Guid id,
        ICustomerRoot root,
        IAddressBook addressBook,
        IReadOnlyList<IConsent> consents)
    {
        return new CustomerAnemicModel(id, root, addressBook, consents);
    }
}
