namespace DiscountCode.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IDiscountCodeAnemicModel : IAnemicModel<IDiscountCode>
{
    IDiscountCodeRoot Root { get; }
    IReadOnlyList<IRedemption> Redemptions { get; }
}
