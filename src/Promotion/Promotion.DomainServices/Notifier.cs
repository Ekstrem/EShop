namespace Promotion.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Promotion.Domain;

public sealed class Notifier : INotifier<IPromotion>
{
    public void Notify<TModel>(AggregateResult<IPromotion, TModel> result)
        where TModel : IAnemicModel<IPromotion>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
