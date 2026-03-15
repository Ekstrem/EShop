namespace DiscountCode.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using DiscountCode.Domain;

public sealed class Notifier : INotifier<IDiscountCode>
{
    public void Notify<TModel>(AggregateResult<IDiscountCode, TModel> result)
        where TModel : IAnemicModel<IDiscountCode>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
