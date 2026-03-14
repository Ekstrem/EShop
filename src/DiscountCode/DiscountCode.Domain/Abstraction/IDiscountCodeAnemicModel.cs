namespace DiscountCode.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IDiscountCodeAnemicModel : IAnemicModel<IDiscountCode>
{
    IDiscountCodeRoot Root { get; }
    IReadOnlyList<IRedemption> Redemptions { get; }
}
