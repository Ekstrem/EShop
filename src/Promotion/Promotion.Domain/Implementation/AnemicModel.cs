namespace Promotion.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Promotion.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<IPromotion>, IPromotionAnemicModel
{
    public IPromotionRoot Root { get; internal set; } = null!;
    public bool AllowStacking { get; internal set; }
}
