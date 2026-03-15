namespace StockItem.DomainServices;

using StockItem.Domain.Abstraction;

public sealed class AggregateProvider
{
    public object Create(IStockItemAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
