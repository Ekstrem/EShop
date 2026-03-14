using Customer.Domain.Abstraction;

namespace Customer.Domain.Implementation;

/// <summary>
/// Immutable implementation of an address value object.
/// </summary>
public sealed class Address : IAddress
{
    private Address(
        string street,
        string city,
        string zipCode,
        string country,
        bool isDefault)
    {
        Street = street;
        City = city;
        ZipCode = zipCode;
        Country = country;
        IsDefault = isDefault;
    }

    public string Street { get; }
    public string City { get; }
    public string ZipCode { get; }
    public string Country { get; }
    public bool IsDefault { get; }

    public static Address CreateInstance(
        string street,
        string city,
        string zipCode,
        string country,
        bool isDefault)
    {
        return new Address(street, city, zipCode, country, isDefault);
    }
}
