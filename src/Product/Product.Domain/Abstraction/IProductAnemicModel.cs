namespace Product.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IProductAnemicModel : IAnemicModel<IProduct>
{
    IProductRoot Root { get; }
    IReadOnlyList<IProductVariant> Variants { get; }
    IReadOnlyList<IProductMedia> Media { get; }
}
