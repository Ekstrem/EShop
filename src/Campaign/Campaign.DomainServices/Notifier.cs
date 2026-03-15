namespace Campaign.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Campaign.Domain;

public sealed class Notifier : INotifier<ICampaign>
{
    public void Notify<TModel>(AggregateResult<ICampaign, TModel> result)
        where TModel : IAnemicModel<ICampaign>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
