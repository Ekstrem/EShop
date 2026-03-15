namespace Promotion.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IPromotionAnemicModel : IAnemicModel<IPromotion>
{
    IPromotionRoot Root { get; }
    bool AllowStacking { get; }
}
