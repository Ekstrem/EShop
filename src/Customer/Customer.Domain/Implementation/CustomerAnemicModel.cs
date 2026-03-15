namespace Customer.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Customer.Domain.Abstraction;

/// <summary>
/// Immutable anemic model for the Customer aggregate.
/// </summary>
public sealed class CustomerAnemicModel : ICustomerAnemicModel
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
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public ICustomerRoot Root { get; }
    public IAddressBook AddressBook { get; }
    public IReadOnlyList<IConsent> Consents { get; }

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject>
        {
            ["Root"] = Root,
            ["AddressBook"] = AddressBook
        };

    public static CustomerAnemicModel CreateInstance(
        Guid id,
        ICustomerRoot root,
        IAddressBook addressBook,
        IReadOnlyList<IConsent> consents)
    {
        return new CustomerAnemicModel(id, root, addressBook, consents);
    }
}
