namespace DiscountCode.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using DiscountCode.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<IDiscountCode>, IDiscountCodeAnemicModel
{
    public IDiscountCodeRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IRedemption> Redemptions { get; internal set; } = new List<IRedemption>();
}
