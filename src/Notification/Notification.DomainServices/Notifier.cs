namespace Notification.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Notification.Domain;

public sealed class Notifier : INotifier<INotification>
{
    public void Notify<TModel>(AggregateResult<INotification, TModel> result)
        where TModel : IAnemicModel<INotification>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
