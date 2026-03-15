namespace AggregateRating.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using AggregateRating.Domain;

public sealed class Notifier : INotifier<IAggregateRating>
{
    public void Notify<TModel>(AggregateResult<IAggregateRating, TModel> result)
        where TModel : IAnemicModel<IAggregateRating>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
