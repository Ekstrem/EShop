namespace StockItem.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using StockItem.Domain;

public sealed class BusAdapter : IBusAdapter<IStockItem>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
