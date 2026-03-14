namespace Notification.DomainServices;

using Hive.SeedWorks.Events;
using Notification.Domain;

public sealed class Notifier : INotifier<INotification>
{
    public Task Notify<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<INotification>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
        return Task.CompletedTask;
    }
}
