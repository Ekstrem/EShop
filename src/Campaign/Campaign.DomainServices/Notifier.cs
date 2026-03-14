namespace Campaign.DomainServices;

using Hive.SeedWorks.Events;
using Campaign.Domain;

public sealed class Notifier : INotifier<ICampaign>
{
    public Task Notify<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<ICampaign>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
        return Task.CompletedTask;
    }
}
