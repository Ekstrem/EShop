namespace Cart.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IShippingAddress : IValueObject
{
    string Street { get; }
    string City { get; }
    string ZipCode { get; }
    string Country { get; }
}
