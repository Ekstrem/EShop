namespace DiscountCode.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using DiscountCode.Domain;

public sealed class Notifier : INotifier<IDiscountCode>
{
    public void Notify<TModel>(AggregateResult<IDiscountCode, TModel> result)
        where TModel : IAnemicModel<IDiscountCode>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
