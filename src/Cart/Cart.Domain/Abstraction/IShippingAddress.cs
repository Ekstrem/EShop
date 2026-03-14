namespace Cart.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IShippingAddress : IValueObject
{
    string Street { get; }
    string City { get; }
    string ZipCode { get; }
    string Country { get; }
}
