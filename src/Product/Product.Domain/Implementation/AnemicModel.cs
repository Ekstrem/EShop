namespace Product.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Product.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<IProduct>, IProductAnemicModel
{
    public IProductRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IProductVariant> Variants { get; internal set; } = new List<IProductVariant>();
    public IReadOnlyList<IProductMedia> Media { get; internal set; } = new List<IProductMedia>();
}
