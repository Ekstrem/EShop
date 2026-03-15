namespace AggregateRating.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using AggregateRating.Domain;

public sealed class Notifier : INotifier<IAggregateRating>
{
    public void Notify<TModel>(AggregateResult<IAggregateRating, TModel> result)
        where TModel : IAnemicModel<IAggregateRating>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
