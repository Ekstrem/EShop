namespace DiscountCode.DomainServices;

using Hive.SeedWorks.Events;
using DiscountCode.Domain;

public sealed class Notifier : INotifier<IDiscountCode>
{
    public Task Notify<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IDiscountCode>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
        return Task.CompletedTask;
    }
}
