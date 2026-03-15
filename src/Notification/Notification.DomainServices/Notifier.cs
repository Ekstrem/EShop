namespace Notification.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Notification.Domain;

public sealed class Notifier : INotifier<INotification>
{
    public void Notify<TModel>(AggregateResult<INotification, TModel> result)
        where TModel : IAnemicModel<INotification>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
