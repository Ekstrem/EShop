namespace AggregateRating.DomainServices;

using Hive.SeedWorks.Events;
using AggregateRating.Domain;

public sealed class Notifier : INotifier<IAggregateRating>
{
    public Task Notify<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IAggregateRating>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
        return Task.CompletedTask;
    }
}
