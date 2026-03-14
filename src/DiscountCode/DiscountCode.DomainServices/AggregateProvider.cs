namespace DiscountCode.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using DiscountCode.Domain;
using DiscountCode.Domain.Abstraction;

public sealed class AggregateProvider : IAggregateProvider<IDiscountCode, IDiscountCodeAnemicModel>
{
    public IAggregate<IDiscountCode, IDiscountCodeAnemicModel> Create(IDiscountCodeAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
