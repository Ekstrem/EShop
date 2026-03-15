namespace Product.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IProductAnemicModel : IAnemicModel<IProduct>
{
    IProductRoot Root { get; }
    IReadOnlyList<IProductVariant> Variants { get; }
    IReadOnlyList<IProductMedia> Media { get; }
}
