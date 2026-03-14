using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Specifications;

/// <summary>
/// Validates that the address book does not exceed the maximum of 10 addresses.
/// </summary>
public sealed class AddressBookLimitValidator : IBusinessOperationValidator<ICustomer, ICustomerAnemicModel>
{
    private const int MaxAddresses = 10;

    private AddressBookLimitValidator()
    {
    }

    public static AddressBookLimitValidator CreateInstance()
    {
        return new AddressBookLimitValidator();
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return model.AddressBook.Addresses.Count <= MaxAddresses;
    }

    public string ErrorMessage => $"Address book cannot contain more than {MaxAddresses} addresses.";
}
