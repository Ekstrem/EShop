namespace Promotion.DomainServices;

using Promotion.Domain.Abstraction;

public sealed class AggregateProvider
{
    public object Create(IPromotionAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
