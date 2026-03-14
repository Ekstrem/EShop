namespace StockItem.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using StockItem.Domain;
using StockItem.Domain.Abstraction;

public sealed class AggregateProvider : IAggregateProvider<IStockItem, IStockItemAnemicModel>
{
    public IAggregate<IStockItem, IStockItemAnemicModel> Create(IStockItemAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
