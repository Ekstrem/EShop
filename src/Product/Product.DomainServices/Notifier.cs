namespace Product.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Product.Domain;

public sealed class Notifier : INotifier<IProduct>
{
    public void Notify<TModel>(AggregateResult<IProduct, TModel> result)
        where TModel : IAnemicModel<IProduct>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
