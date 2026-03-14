namespace Promotion.DomainServices;

using Hive.SeedWorks.Events;
using Promotion.Domain;

public sealed class Notifier : INotifier<IPromotion>
{
    public Task Notify<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IPromotion>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
        return Task.CompletedTask;
    }
}
