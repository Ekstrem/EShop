namespace Product.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Product.Domain;
using Product.Domain.Abstraction;

public sealed class AggregateProvider : IAggregateProvider<IProduct, IProductAnemicModel>
{
    public IAggregate<IProduct, IProductAnemicModel> Create(IProductAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
