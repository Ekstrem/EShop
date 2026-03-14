namespace Cart.Domain.Implementation;

using Cart.Domain.Abstraction;

internal sealed class ShippingAddress : IShippingAddress
{
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    private ShippingAddress() { }

    public static IShippingAddress CreateInstance(
        string street,
        string city,
        string zipCode,
        string country)
        => new ShippingAddress
        {
            Street = street,
            City = city,
            ZipCode = zipCode,
            Country = country
        };
}
