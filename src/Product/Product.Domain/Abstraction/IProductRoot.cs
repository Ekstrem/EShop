namespace Product.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IProductRoot : IValueObject
{
    string Name { get; }
    string Description { get; }
    Guid CategoryId { get; }
    string Status { get; }
}
