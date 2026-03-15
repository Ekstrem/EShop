namespace Review.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Review.Domain;

public sealed class Notifier : INotifier<IReview>
{
    public void Notify<TModel>(AggregateResult<IReview, TModel> result)
        where TModel : IAnemicModel<IReview>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
