namespace Category.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Category.Domain;
using Category.Domain.Abstraction;

public sealed class AggregateProvider : IAggregateProvider<ICategory, ICategoryAnemicModel>
{
    public IAggregate<ICategory, ICategoryAnemicModel> Create(ICategoryAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
