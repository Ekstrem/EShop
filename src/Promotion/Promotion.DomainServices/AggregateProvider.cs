namespace Promotion.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Promotion.Domain;
using Promotion.Domain.Abstraction;

public sealed class AggregateProvider : IAggregateProvider<IPromotion, IPromotionAnemicModel>
{
    public IAggregate<IPromotion, IPromotionAnemicModel> Create(IPromotionAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
