namespace Promotion.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Promotion.Domain;

public sealed class Notifier : INotifier<IPromotion>
{
    public void Notify<TModel>(AggregateResult<IPromotion, TModel> result)
        where TModel : IAnemicModel<IPromotion>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
