namespace StockItem.DomainServices;

using Hive.SeedWorks.Events;
using StockItem.Domain;

public sealed class BusAdapter : IBusAdapter<IStockItem>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IStockItem>
    {
        return Task.CompletedTask;
    }
}
