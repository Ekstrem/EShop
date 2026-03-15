namespace StockItem.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using StockItem.Domain;

public sealed class Notifier : INotifier<IStockItem>
{
    public void Notify<TModel>(AggregateResult<IStockItem, TModel> result)
        where TModel : IAnemicModel<IStockItem>
    {
    }
}
