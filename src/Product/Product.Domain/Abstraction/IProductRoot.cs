namespace Product.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IProductRoot : IAggregateRoot<IProduct>
{
    string Name { get; }
    string Description { get; }
    Guid CategoryId { get; }
    string Status { get; }
}
