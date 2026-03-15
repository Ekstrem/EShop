namespace Customer.Domain.Specifications;

using Customer.Domain.Abstraction;

/// <summary>
/// Validates that the address book does not exceed the maximum of 10 addresses.
/// </summary>
public sealed class AddressBookLimitValidator
{
    private const int MaxAddresses = 10;

    private AddressBookLimitValidator() { }

    public static AddressBookLimitValidator CreateInstance()
    {
        return new AddressBookLimitValidator();
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return model.AddressBook.Addresses.Count <= MaxAddresses;
    }

    public string Reason => $"Address book cannot contain more than {MaxAddresses} addresses.";
}
