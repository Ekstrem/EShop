namespace StockItem.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using StockItem.Domain;

public sealed class Notifier : INotifier<IStockItem>
{
    public void Notify<TModel>(AggregateResult<IStockItem, TModel> result)
        where TModel : IAnemicModel<IStockItem>
    {
    }
}
