namespace Promotion.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IPromotionAnemicModel : IAnemicModel<IPromotion>
{
    IPromotionRoot Root { get; }
    bool AllowStacking { get; }
}
