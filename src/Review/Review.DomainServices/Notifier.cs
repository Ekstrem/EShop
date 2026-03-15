namespace Review.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Review.Domain;

public sealed class Notifier : INotifier<IReview>
{
    public void Notify<TModel>(AggregateResult<IReview, TModel> result)
        where TModel : IAnemicModel<IReview>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
