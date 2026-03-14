using Customer.Domain.Abstraction;

namespace Customer.Domain.Implementation;

/// <summary>
/// Immutable implementation of the address book value object.
/// </summary>
public sealed class AddressBook : IAddressBook
{
    private AddressBook(IReadOnlyList<IAddress> addresses)
    {
        Addresses = addresses;
    }

    public IReadOnlyList<IAddress> Addresses { get; }

    public static AddressBook CreateInstance(IReadOnlyList<IAddress> addresses)
    {
        return new AddressBook(addresses);
    }

    public static AddressBook Empty()
    {
        return new AddressBook(Array.Empty<IAddress>());
    }
}
