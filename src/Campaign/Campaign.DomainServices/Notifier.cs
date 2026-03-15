namespace Campaign.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Campaign.Domain;

public sealed class Notifier : INotifier<ICampaign>
{
    public void Notify<TModel>(AggregateResult<ICampaign, TModel> result)
        where TModel : IAnemicModel<ICampaign>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
