namespace Product.DomainServices;

using Hive.SeedWorks.Events;
using Product.Domain;

public sealed class Notifier : INotifier<IProduct>
{
    public Task Notify<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IProduct>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
        return Task.CompletedTask;
    }
}
