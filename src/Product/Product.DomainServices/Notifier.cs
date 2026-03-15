namespace Product.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Product.Domain;

public sealed class Notifier : INotifier<IProduct>
{
    public void Notify<TModel>(AggregateResult<IProduct, TModel> result)
        where TModel : IAnemicModel<IProduct>
    {
        // Push notifications to subscribers (SignalR, WebSocket, etc.)
    }
}
