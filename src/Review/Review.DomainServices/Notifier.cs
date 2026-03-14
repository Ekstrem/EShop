namespace Review.DomainServices;

using Hive.SeedWorks.Events;
using Review.Domain;

public sealed class Notifier : INotifier<IReview>
{
    public Task Notify<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IReview>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
        return Task.CompletedTask;
    }
}
