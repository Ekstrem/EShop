namespace StockItem.DomainServices;

using Hive.SeedWorks.Events;
using StockItem.Domain;

public sealed class Notifier : INotifier<IStockItem>
{
    public Task Notify<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IStockItem>
    {
        return Task.CompletedTask;
    }
}
